using System;
using System.Collections.Generic;
using UnityEngine;

public static class EMove
{
    public static int Grounded = 0;
    public static int OnAir = 1;
    public static int Climbing = 2;
}

public class PlayerMovementController : MonoBehaviour
{
    #region Components
    public StateMachine<MeleeBaseState> combatMachine;
    public StateMachine<PlayerBaseMovementState> movementMachine;
    private PlayerData data;
    private Transform _GFX;
    private PlayerMove playerMove => data.playerMove;
    private PlayerJump playerJump => data.playerJump;
    private PlayerDash playerDash => data.playerDash;
    private GroundChecker groundChecker;
    private CustomGravity gravManager;
    private AnimationManager animationManager;
    private Rigidbody2D rb;
    private CapsuleCollider2D playerHitbox => data.playerHitbox;
    private PlayerBaseMovementState currentState => movementMachine.CurrentState;
    #endregion

    private Vector2 playerPos{
        get{
            return rb.position + playerHitbox.offset;
        }
    }

    List<PlayerBaseMovementState> movementStates;

    public void SetController(PlayerData _data)
    {
        data = _data;
        SetDefaultVars();
    }

    private void SetDefaultVars()
    {
        groundChecker = data.groundChecker;
        gravManager = data.customGravity;
        _GFX = data.GFX;
        combatMachine = data.combatMachine;
        movementMachine = data.movementMachine;
        movementMachine.Init();
        animationManager = data.animationManager;
        rb = data.rb;

        SetupMachine();

        data.collisionListener.enterEvents += WallSlideEnter;
        data.collisionListener.stayEvents += WallSlideStay;
        data.collisionListener.exitEvents += WallSlideExit;
    }

    private void SetupMachine(){
        SetupStates();
        SetupStateBehavior();
        SetUpTransitions();
        movementMachine.mainStateType = movementStates[EMove.Grounded];
        movementMachine.SetNextStateToMain();
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
        foreach (PlayerBaseMovementState movementState in movementStates)
        {
            movementState.Init(data);
        }
    }

    private void SetUpTransitions()
    {
        movementMachine.anyTransitions = new()
        {
            new(() => isOnAir(), movementStates[EMove.OnAir]),
            new(() => isClimbing(), movementStates[EMove.Climbing]),
            new(() => isGrounded(), movementStates[EMove.Grounded])
        };
    }

    private bool isOnAir()
    {
        //Debug.Log("Is On Air: " + (!isGrounded() && !isClimbing()));
        return !isGrounded() && !isClimbing();
    }

    private bool isGrounded()
    {
        return groundChecker.isGrounded();
    }

    private bool isClimbing()
    {
        return isClimbableWall() && Mathf.Abs(data.movementInput.x) > 0.01f;
    }

    void FixedUpdate()
    {
        CheckAhead();
        if (data.isDashing && isWallAhead())
        {
            playerDash.OnHitWall();
        }
        movementMachine.FixedHandle();
    }

    private bool IsMovementState(Type type)
    {
        return movementMachine.CurrentState.GetType() == type;
    }

    public void UseMove()
    {
        if (Mathf.Abs(data.movementInput.x) > 0.01f)
        {
            flipCharacter(data.movementInput);
        }
        currentState.UseMove();
    }

    public void OnMove(Vector2 _movementInput)
    {
        data.movementInput = _movementInput;
    }

    public void OnJump()
    {
        if (!data.canJump) return;
        currentState.UseJump();
    }

    public void ReleaseJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.4f);
        data.jumpRelease = true;
        rememberVelocity *= 0.4f;
    }

    public void UseSlide()
    {
        currentState.UseSlide();
    }

    public void UseDash()
    {
        if (IsMovementState(typeof(ClimbingState))) return;
        playerDash.DashCharacter();
    }

    private Collider2D[] getAllBoxCast(float distance)
    {
        return Physics2D.OverlapBoxAll(
            rb.position + playerHitbox.offset + Vector2.right * _GFX.localScale.x * distance,
            new Vector2(0.1f, playerHitbox.size.y - 0.1f),
            0
            );
    }

    private RaycastHit2D[] CastForward(Vector2 origin, float distance)
    {
        return Physics2D.RaycastAll(
            origin,
            Vector2.right * _GFX.localScale.x,
            distance
            );
    }

    private RaycastHit2D[] CastUpward(Vector2 origin, float distance)
    {
        return Physics2D.RaycastAll(
            origin,
            Vector2.up,
            distance
        );
    }

    public bool isWallAhead()
    {
        return data.upperHit || data.lowerHit || data.midHit;
    }

    #region Ledge Slide
    private bool applySlide = false;
    private Vector2 rememberVelocity;
    List<Vector2> pointToDraws = new List<Vector2>();
    List<Ray> rayToDraws = new();

    private bool ShouldWallSlide(Collision2D collision)
    {
        return canSlideWall()
        && collision.contacts[0].point.y > rb.position.y + playerHitbox.offset.y + playerHitbox.size.y / 2 - playerHitbox.size.x / 2 + 0.01
        && Mathf.Sign(rb.velocity.x) == Mathf.Sign(_GFX.localScale.x);
    }

    private void WallSlideEnter(Collision2D collision)
    {
        if (ShouldWallSlide(collision))
        {
            applySlide = true;
            Vector2 normalImpulse = collision.contacts[0].normal;
            rememberVelocity = collision.relativeVelocity * normalImpulse.y;
            rb.velocity -= new Vector2(-2f * _GFX.localScale.x, 0f);
            data.isAccelGrav = false;
            data.customGravity.useGravity = false;
            data.canMove = false;
        }
    }

    private void WallSlideStay(Collision2D collision)
    {
    }

    private void WallSlideExit(Collision2D collision)
    {
        if (applySlide)
        {
            applySlide = false;
            data.canMove = true;
            rb.velocity += new Vector2(0f,rememberVelocity.y);
            rememberVelocity = Vector2.zero;
            data.isAccelGrav = true;
            data.customGravity.useGravity = true;
        }
    }

    public bool canSlideWall()
    {
        bool isHitForward = false;
        RaycastHit2D[] forwardHits = CastUpward(
            playerPos + new Vector2(playerHitbox.size.x / 2 + 0.1f, 0f) * _GFX.localScale.x,
            playerHitbox.size.y / 2 + 0.2f
        );

        foreach (RaycastHit2D hit in forwardHits)
        {
            if (hit.transform.CompareTag("Object"))
            {
                isHitForward = true;
                break;
            }
        }

        return !isHitForward;
    }
    #endregion

    #region Climb
    public bool isClimbableWall()
    {
        return data.upperHit && data.lowerHit;
    }

    private void CheckAhead()
    {
        data.upperHit = data.lowerHit = data.midHit = data.topHit = data.bottomHit = false;
        float distance = playerHitbox.size.x * 1 / 2 + 0.2f;
        RaycastHit2D[] upperHits = CastForward(data.ClimbUpper.position, distance),
        lowerHits = CastForward(data.ClimbLower.position, distance),
        midHits = CastForward(playerPos, distance - 0.2f),
        topHits = CastForward(playerPos + new Vector2(0,playerHitbox.size.y/2), distance),
        bottomHits = CastForward(playerPos + new Vector2(0,-playerHitbox.size.y/2), distance);

        foreach (RaycastHit2D hit in upperHits)
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Object"))
            {
                data.upperHit = true;
                break;
            }
        }
        foreach (RaycastHit2D hit in lowerHits)
        {
            if (hit.transform.CompareTag("Object"))
            {
                data.lowerHit = true;
                break;
            }
        }
        foreach (RaycastHit2D hit in midHits)
        {
            if (hit.transform.CompareTag("Object"))
            {
                data.midHit = true;
                break;
            }
        }
        foreach (RaycastHit2D hit in topHits)
        {
            if (hit.transform.CompareTag("Object"))
            {
                data.topHit = true;
                break;
            }
        }
        foreach (RaycastHit2D hit in bottomHits)
        {
            if (hit.transform.CompareTag("Object"))
            {
                data.bottomHit = true;
                break;
            }
        }
    }
    #endregion

    private void flipCharacter(Vector2 _movementInput)
    {
        if (!data.canFlip) return;
        if (_movementInput.x > 0)
        {
            _GFX.localScale = new Vector3(1, 1, 1);
        }
        else if (_movementInput.x < 0)
        {
            _GFX.localScale = new Vector3(-1, 1, 1);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (rb != null)
        {
            /*Gizmos.DrawRay(
                rb.position + playerHitbox.offset,
                Vector2.right * _GFX.localScale.x * 1f
                );
            Gizmos.DrawCube(
                rb.position + playerHitbox.offset + Vector2.up * Mathf.Sign(data.movementInput.y) * data.playerHitbox.size.y * 1 / 2,
                new Vector2(data.playerHitbox.size.x - 0.001f, 0.1f)
                );
            */
            Gizmos.DrawRay(
                data.ClimbUpper.position,
                Vector2.right * _GFX.localScale.x * (data.playerHitbox.size.x * 1 / 2 + 0.1f)
            );
            Gizmos.DrawRay(
                rb.position + playerHitbox.offset + new Vector2(playerHitbox.size.x / 2, 0f) * _GFX.localScale.x,
                Vector2.up * (playerHitbox.size.y / 2 + 0.2f)
            );
            foreach (Vector2 p in pointToDraws)
            {
                Gizmos.DrawSphere(p, 0.1f);
            }
            foreach(var x in rayToDraws){
                Gizmos.DrawRay(x);
            }
        }
    }
}
