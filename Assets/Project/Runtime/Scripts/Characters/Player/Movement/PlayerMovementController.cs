using System;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class EMove {
    public static int Jump = 1;
    public static int Run = 0;
    public static int Crouch = 2;
}

public class PlayerMovementController : MonoBehaviour
{
    #region Components
    private StateMachine combatMachine;
    private Vector2 movementInput;
    private Animator animator;
    private PlayerData data;
    private Transform _GFX;
    public PlayerMove _playerMove;
    public PlayerJump _playerJump;
    private GroundChecker groundChecker;
    private CustomGravity gravManager;
    private AnimationManager animationManager;
    private Rigidbody2D rb;
    private CapsuleCollider2D playerHitbox => data.playerHitbox;
    #endregion

    #region Jump
    private float coyoteTime => data.coyoteTime;
    private float jumpPhase = 0;
    private float jumpStartTime, jumpPeakTime, jumpEndTime, jumpExpectedPeakTime;
    private float jumpAirMoveTime => data.jumpAirMoveTime;
    private float jumpMoveMult = 0.2f;
    private bool AirHover = false, WallClimbing  = false;
    #endregion

    private float gravAccel => data.gravAccel;
    private float defGrav => data.defGrav;
    private float globalGravity => CustomGravity.globalGravity;

    public void SetController(PlayerData _data)
    {
        data = _data;
        SetDefaultVars();
    }

    private void SetDefaultVars()
    {
        animator = data.animator;
        _playerMove = new PlayerMove(data);
        _playerJump = new PlayerJump(data);
        movementInput = data.movementInput;
        groundChecker = data.groundChecker;
        gravManager = data.customGravity;
        _GFX = data.GFX;
        combatMachine = data.combatMachine;
        animationManager = data.animationManager;
        rb = data.rb;

        jumpExpectedPeakTime = getJumpPeakTime();
    }

    private bool isState(Type type)
    {
        if (combatMachine.CurrentState == null) return false;
        return combatMachine.CurrentState.GetType() == type;
    }

    private bool isOutOfCombat()
    {
        return isState(typeof(MeleeEntryState)) || isState(typeof(CombatIdleState));
    }

    private void HandleDescend()
    {
        animationManager.ForceAdd(2, "Fall");
        jumpPhase = 2;
    }
    
    bool peaked = false;

    void Update(){
        if(!groundChecker.isGrounded()){
            if (data.rb.velocity.y <= 0.01f)
            {
                if(!peaked){
                    peaked = true;
                    jumpPeakTime = (float)Time.timeAsDouble;
                    Debug.Log("Time Took To Peak" + (jumpPeakTime-jumpStartTime));
                }
            }
        }else{
            peaked = false;
        }
    }

    void FixedUpdate()
    {
        if (!groundChecker.isGrounded())
        {
            if (jumpPhase == 1)
            {
                jumpPhase = 2;
            }
            
            if(!AirHover && !WallClimbing) gravManager.gravityScale += gravAccel * Time.fixedDeltaTime;
            
            if (data.rb.velocity.y < 0f)
            {
                HandleDescend();
            }
        }
        else
        {
            //reset number of jumps when landing
            data.jumpsLeft = data.numberOfJumps;

            if (jumpPhase == 2)
            {
                jumpEndTime = (float)Time.timeAsDouble;
                Debug.Log("Jump Ended At " + jumpEndTime);
                Debug.Log("Total Time Took to Land " + (jumpEndTime-jumpStartTime));
                animationManager.RemoveAnim(2);
                jumpPhase = 0;
            }
            if (isOutOfCombat())
            {
                gravManager.gravityScale = data.defGrav;
            }
        }

        if(isWallAhead() && Mathf.Abs(movementInput.x) > 0.01f){
            if(!WallClimbing) {
                StopJump();
                WallClimbing = true;

                animationManager.ForceAdd(3, "WallHang");
            }
        }else if(WallClimbing && (!isWallAhead() || !(Mathf.Abs(movementInput.x) > 0.01f))){
            if(animationManager.isAnim(3, "WallHang")) animationManager.RemoveAnim(3);
            WallClimbing = false;
        }

        if(WallClimbing){
            data.customGravity.gravityScale = data.defGrav/10;
        }
    }

    private bool isGrounded()
    {
        return groundChecker.isGrounded();
    }

    public void UseMove()
    {
        if (movementInput.magnitude > 0.01f)
        {
            float runMult = isGrounded() ? 1 : jumpMoveMult;
            flipCharacter(movementInput);
            _playerMove.moveCharacter(movementInput * runMult);
        }
        else
        {
            if (isGrounded()) _playerMove.brakeMovement(movementInput);
        }
    }

    public void OnMove(Vector2 _movementInput)
    {
        movementInput = _movementInput;
        if (movementInput.magnitude > 0f)
        {
            animationManager.AddAnim(1, "Run");
            movementInput.Normalize();
        }
        else
        {
            animationManager.RemoveAnim(1);
            animator.speed = 1f;
        }
    }

    private Coroutine JumpRoutine;

    public void OnJump()
    {
        if (data.jumpsLeft > 0 || groundChecker.isGrounded())
        {
            Debug.Log("Expected Peak Time: " + getJumpPeakTime());
            StopJump();
            JumpRoutine = StartCoroutine(loopJump());
        }
    }

    public bool jumpRelease = true;

    private float getJumpPeakTime(){
        float res = Mathf.Sqrt((2f*data.jumpPower*gravAccel + globalGravity)/(globalGravity*gravAccel*gravAccel)) - 1f/gravAccel;
        //return Mathf.Sqrt(2f*data.jumpPower/(globalGravity*gravAccel) +1f/(gravAccel*gravAccel))- 1f/gravAccel;
        return res;
    }

    private bool JumpReleased()
    {
        return jumpRelease == true;
    }

    public void ReleaseJump()
    {
        rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*0.6f);
        Debug.Log("Jump Released");
        jumpRelease = true;
    }
    private void StopJump(){
        if(JumpRoutine == null) return;
        if(AirHover){
            AirHover = false;
            jumpPhase = 2;
            data.gravAccel *= 2;
            jumpMoveMult = 0.2f;
        }
        animationManager.RemoveAnim(2);
        StopCoroutine(JumpRoutine);
    }

    private IEnumerator loopJump()
    {
        jumpRelease = false;
        gravManager.gravityScale = data.defGrav;
        animationManager.ForceAdd(2, "Jump");
        yield return new WaitForFixedUpdate();
        _playerJump.jumpStart();
        jumpPhase = 1;
        jumpStartTime = (float)Time.timeAsDouble;
        Debug.Log("Jump Started At " + jumpStartTime);
        yield return new WaitForSeconds(data.anims["Jump"].length);
        animationManager.ForceAdd(2, "JumpLoop");
        yield return new WaitUntil(() => (float)Time.timeAsDouble-jumpStartTime >= jumpExpectedPeakTime-jumpAirMoveTime/2);
        animationManager.ForceAdd(2, "AirFloatLoop");
        AirHover = true;
        data.gravAccel /= 2;
        jumpMoveMult = 1.2f;
        yield return new WaitForSeconds(jumpAirMoveTime);
        AirHover = false;
        jumpPhase = 2;
        data.gravAccel *= 2;
        jumpMoveMult = 0.2f;
        yield return new WaitUntil(() => groundChecker.isGrounded());
    }

    public void OnCrouch()
    {
        _playerMove.crouchCharacter();
    }

    private void flipCharacter(Vector2 _movementInput)
    {
        if (_movementInput.x > 0)
        {
            _GFX.localScale = new Vector3(1, 1, 1);
        }
        else if (_movementInput.x < 0)
        {
            _GFX.localScale = new Vector3(-1, 1, 1);
        }
    }

    private RaycastHit2D[] getAllRayCast(float distance){
        return Physics2D.RaycastAll(rb.position + playerHitbox.offset, Vector2.right * _GFX.localScale.x, distance);
    }

    public bool isWallAhead(){
        RaycastHit2D[] raycastHit2Ds = getAllRayCast(1f);
        bool canClimb = false;
        foreach(RaycastHit2D hit in raycastHit2Ds){
            if(hit.transform.CompareTag("Object")){
                canClimb = true;
                break;
            }
        }
        return canClimb;
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        if(rb != null) Gizmos.DrawRay(rb.position + playerHitbox.offset, Vector2.right * _GFX.localScale.x * 1f);
    }
}
