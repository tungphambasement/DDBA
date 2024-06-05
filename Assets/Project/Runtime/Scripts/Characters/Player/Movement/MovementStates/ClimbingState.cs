using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ClimbingState : PlayerBaseMovementState
{
    PlayerMove playerMove => data.playerMove;
    PlayerJump playerJump => data.playerJump;

    public override void Init(PlayerData data)
    {
        base.Init(data);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //Debug.Log("Entered Climbing");
        data.customGravity.gravityScale = 0;
        animationManager.ForceAdd(3, "WallCling");
        rb.velocity = Vector2.zero;
    }
    private bool canClimb(){
        if(data.movementInput.y > 0.1f && data.topHit) return true;
        else if(data.movementInput.y < -0.1f && data.bottomHit) return true;
        return false;
    }

    public override void OnFixedHandle()
    {
        base.OnFixedHandle();
        if (canClimb())
        {
            playerMove.ClimbCharacter();
        }
        else
        {
            playerMove.StopClimbCharacter();
            animationManager.AddAnim(3, "WallCling");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        playerMove.StopClimbCharacter();
        animationManager.RemoveAnim(3);
        customGravity.gravityScale = data.defGrav;
        //Debug.Log(Time.timeAsDouble + " Exited Climb State");
    }

    public override void UseSlide()
    {
    }

    public override void UseJump()
    {
        base.UseJump();
        playerJump.CancelJump(data.JumpRoutine);
        data.JumpRoutine = data.StartCoroutine(loopJump());
    }

    private float getJumpPeakTime()
    {
        float res = data.jumpPower / data.customGravity.gravityScale / CustomGravity.globalGravity;
        return res;
    }

    private IEnumerator loopJump()
    {
        //Start
        playerJump.jumpStart(30);
        float jumpStartTime = (float)Time.timeAsDouble;

        data.canFlip = false;
        yield return new WaitUntil(() => (float)Time.timeAsDouble - jumpStartTime >= getJumpPeakTime() - data.jumpAirMoveTime / 2 - 0.1f || data.jumpRelease);
        data.canFlip = true;
        //Hang (Hover)
        animationManager.ForceAdd(2, "AirFloatLoop");
        playerJump.ToggleJumpHang(1.5f);
        float AirMoveTime = Time.time;
        yield return new WaitUntil(()=>(Time.time-AirMoveTime>data.jumpAirMoveTime+0.1f) || data.groundChecker.isGrounded() );

        //Unhang (Unhover)
        playerJump.ToggleJumpHang(1);
        animationManager.SafeRemove(2, "AirFloatLoop");

        data.jumpRelease = false;
        data.JumpRoutine = null;
    }
}
