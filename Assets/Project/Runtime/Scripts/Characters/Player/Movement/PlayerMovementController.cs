using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EMove {
    public static int Grounded = 0;
    public static int OnAir = 1;
    public static int Climbing = 2;
}

public class PlayerMovementController : MonoBehaviour
{
    #region Components
    public StateMachine<MeleeBaseState> combatMachine;
    public StateMachine<PlayerBaseMovementState> movementMachine;
    private Animator animator;
    private PlayerData data;
    private Transform _GFX;
    private PlayerMove playerMove => data.playerMove;
    private PlayerJump playerJump  => data.playerJump;
    private PlayerDash playerDash => data.playerDash;
    private GroundChecker groundChecker;
    private CustomGravity gravManager;
    private AnimationManager animationManager;
    private Rigidbody2D rb;
    private CapsuleCollider2D playerHitbox => data.playerHitbox;
    private PlayerBaseMovementState currentState => movementMachine.CurrentState; 
    #endregion

    #region Jump
    private float coyoteTime => data.coyoteTime;
    private float jumpPhase => data.jumpPhase;
    private float jumpStartTime, jumpPeakTime, jumpEndTime, jumpExpectedPeakTime;
    private float jumpAirMoveTime => data.jumpAirMoveTime;
    private float jumpDegree => data.jumpDegree;
    #endregion

    private float gravAccel => data.gravAccel;
    private float defGrav => data.defGrav;
    private float globalGravity => CustomGravity.globalGravity;

    List<PlayerBaseMovementState> movementStates;

    public void SetController(PlayerData _data)
    {
        data = _data;
        SetDefaultVars();
    }

    private void SetDefaultVars()
    {
        animator = data.animator;
        groundChecker = data.groundChecker;
        gravManager = data.customGravity;
        _GFX = data.GFX;
        combatMachine = data.combatMachine;
        movementMachine = data.movementMachine;
        movementMachine.Init();
        animationManager = data.animationManager;
        rb = data.rb;
        SetupStates();
        SetupStateBehavior();
        movementMachine.mainStateType = movementStates[EMove.Grounded];
        movementMachine.SetNextStateToMain();

        jumpExpectedPeakTime = getJumpPeakTime();
    }

    private void SetupStates()
    {
        movementStates = new List<PlayerBaseMovementState>
        {
            new GroundedState(),
            new OnAirState(),
            new ClimbingState()
        };
    }

    private void SetupStateBehavior()
    {
        foreach(PlayerBaseMovementState movementState in movementStates){
            movementState.Init(data);
        }
    }

    void Update(){
        
    }

    private bool isOnAir(){
        return !isGrounded()  && !isClimbing();
    }

    private bool isGrounded(){
        return groundChecker.isGrounded();
    }

    private bool isClimbing(){
        return isClimbableWall() && Mathf.Abs(data.movementInput.x) > 0.01f;
    }

    void FixedUpdate()
    {
        if(data.isDashing && isWallAhead()){
            playerDash.OnHitWall();
        }
        movementMachine.FixedHandle();
        if (isOnAir())
        {
            if(!IsMovementState(typeof(OnAirState)))
                movementMachine.SetNextState(movementStates[EMove.OnAir]);
        }else if(isClimbing()){
            if(!IsMovementState(typeof(ClimbingState))) {
                movementMachine.SetNextState(movementStates[EMove.Climbing]);
                playerJump.CancelJump(JumpRoutine);
            }
        }
        else if(isGrounded())
        {
            if(!IsMovementState(typeof(GroundedState))){
                movementMachine.SetNextState(movementStates[EMove.Grounded]);
            }
        }
    }

    private bool IsMovementState(Type type){
        return movementMachine.CurrentState.GetType() == type;
    }

    public void UseMove()
    {
        currentState.UseMove();
    }

    public void OnMove(Vector2 _movementInput)
    {
        data.movementInput = _movementInput;
        if (Mathf.Abs(data.movementInput.x) > 0.01f)
        {
            flipCharacter(data.movementInput);
        }
    }

    private Coroutine JumpRoutine;

    public void OnJump()
    {
        if(!data.canJump) return;
        if (data.jumpsLeft > 0)
        {
            currentState.UseJump();
            if(IsMovementState(typeof(OnAirState)) && coyoteTime < 0.01f) 
                data.jumpsLeft--;
            //Debug.Log("Expected Peak Time: " + getJumpPeakTime());
            data.jumpDegree = IsMovementState(typeof(ClimbingState)) ? 35 : 0;
            playerJump.CancelJump(JumpRoutine);
            JumpRoutine = data.StartCoroutine(loopJump());
        }
    }

    private float getJumpPeakTime(){
        //float res = Mathf.Sqrt((2f*data.jumpPower*gravAccel + globalGravity)/(globalGravity*gravAccel*gravAccel)) - 1f/gravAccel;
        float res = data.jumpPower / data.customGravity.gravityScale / globalGravity; 
        return res;
    }

    public void ReleaseJump()
    {
        rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*0.4f);
        //Debug.Log("Jump Released");
        data.jumpRelease = true;
    }

    private IEnumerator loopJump()
    {   
        //Start
        playerJump.jumpStart(jumpDegree);
        jumpStartTime = (float)Time.timeAsDouble;

        //yield return new WaitForSeconds(data.anims["Jump"].length);

        //Loop
        //animationManager.ForceAdd(2, "JumpLoop");

        yield return new WaitUntil(() => ((float)Time.timeAsDouble-jumpStartTime >= jumpExpectedPeakTime-jumpAirMoveTime/2||data.jumpRelease));

        //Hang (Hover)
        animationManager.ForceAdd(2, "AirFloatLoop");
        playerJump.ToggleJumpHang();

        yield return new WaitForSeconds(jumpAirMoveTime);

        //Unhang (Unhover)
        playerJump.ToggleJumpHang();

        data.jumpRelease = false;
        JumpRoutine = null;
    }

    public void UseSlide()
    {
        currentState.UseSlide();
    }


    public void UseDash(){
        if(IsMovementState(typeof(ClimbingState))) return;
        playerDash.DashCharacter();
    }

    private Collider2D[] getAllBoxCast(float distance){
        return Physics2D.OverlapBoxAll(
            rb.position + playerHitbox.offset + Vector2.right * _GFX.localScale.x * distance, 
            new Vector2(0.1f, playerHitbox.size.y - 0.1f),
            0
            );
    }
    
    private RaycastHit2D[] getAllRayCast(Vector2 origin, float distance){
        return Physics2D.RaycastAll(
            origin, 
            Vector2.right * _GFX.localScale.x,
            distance
            );
    }

    public bool isWallAhead(){
        Collider2D[] colliderHit2D = getAllBoxCast(1f);
        bool canClimb = false;
        foreach(Collider2D hit in colliderHit2D){
            if(hit.transform.CompareTag("Object")){
                canClimb = true;
                break;
            }
        }
        return canClimb;
    }

    public bool isClimbableWall(){
        float distance = data.playerHitbox.size.x*1/2+0.2f;
        RaycastHit2D[] upperHits = getAllRayCast(data.ClimbUpper.position,distance), lowerHits = getAllRayCast(data.ClimbLower.position,distance);
        int canClimbWall = 0;
        foreach(RaycastHit2D hit in upperHits){
            //Debug.Log(hit.transform.name);
            if(hit.transform.CompareTag("Object")){
                canClimbWall++;
                break;
            }
        }
        foreach(RaycastHit2D hit in lowerHits){
            if(hit.transform.CompareTag("Object")){
                canClimbWall++;
                break;
            }
        }
        return canClimbWall == 2;
    }

    private void flipCharacter(Vector2 _movementInput)
    {
        if(!data.canFlip) return;
        if (_movementInput.x > 0)
        {
            _GFX.localScale = new Vector3(1, 1, 1);
        }
        else if (_movementInput.x < 0)
        {
            _GFX.localScale = new Vector3(-1, 1, 1);
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        if(rb != null){ 
            Gizmos.DrawRay(
                rb.position + playerHitbox.offset, 
                Vector2.right * _GFX.localScale.x * 1f
                );
            Gizmos.DrawCube(
                rb.position + playerHitbox.offset + Vector2.up*Mathf.Sign(data.movementInput.y)*data.playerHitbox.size.y*1/2,
                new Vector2(data.playerHitbox.size.x-0.001f,0.1f)
                );
            Gizmos.DrawRay(
                data.ClimbUpper.position,
                Vector2.right * _GFX.localScale.x * (data.playerHitbox.size.x*1/2+0.1f)
            );
        }
    }
}
