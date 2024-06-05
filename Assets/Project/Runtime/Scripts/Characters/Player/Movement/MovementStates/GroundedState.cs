using System;
using System.Collections;
using UnityEngine;

public class GroundedState : PlayerBaseMovementState
{
    PlayerMove playerMove => data.playerMove;
    PlayerJump  playerJump=> data.playerJump;
    public override void Init(PlayerData data)
    {
        base.Init(data);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        data.customGravity.gravityScale = data.defGrav;
    }

    public override void OnFixedHandle()
    {
        base.OnFixedHandle();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void UseMove()
    {
        base.UseMove();
        if (data.movementInput.y < -0.01f)
        {
            playerMove.CrouchCharacter();
        }
        else
        {
            playerMove.UnCrouchCharacter();
            if (Mathf.Abs(data.movementInput.x) > 0.01f && data.canMove)
            {
                playerMove.RunCharacter(data.movementInput);
            }
            else if (!data.isDashing && !data.isSliding)
                playerMove.BrakeCharacter(data.movementInput);
        }
    }

    public override void UseSlide()
    {
        base.UseSlide();
        playerMove.SlideCharacter();
    }

    public override void UseJump()
    {
        base.UseJump();
        if (!data.canJump) return;
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
        data.jumpRelease = false;
        //Start
        playerJump.jumpStart(0);
        float jumpStartTime = (float)Time.timeAsDouble;
        //Debug.Log(data.jumpRelease + " " + jumpStartTime + getJumpPeakTime());
        yield return new WaitUntil(() => (float)Time.timeAsDouble - jumpStartTime >= getJumpPeakTime() - data.jumpAirMoveTime / 2 || data.jumpRelease);
        animationManager.ForceAdd(2, "AirFloatLoop");
        playerJump.ToggleJumpHang(1.2f);
        float AirMoveTime = Time.time;
        yield return new WaitUntil(()=>(Time.time-AirMoveTime>data.jumpAirMoveTime) || data.groundChecker.isGrounded() );

        //Unhang (Unhover)
        playerJump.ToggleJumpHang(1.2f);
        animationManager.SafeRemove(2, "AirFloatLoop");
        //Debug.Log("Jump release is false");
        data.jumpRelease = false;
        data.JumpRoutine = null;
    }
}