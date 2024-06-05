

using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;

public class OnAirState : PlayerBaseMovementState{
    PlayerMove playerMove => data.playerMove;
    PlayerJump playerJump => data.playerJump;
    private bool isAirHover => data.AirHover;
    
    public override void Init(PlayerData data)
    {
        base.Init(data);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //Debug.Log(Time.timeAsDouble + " Entered On Air");
    }

    public override void OnFixedHandle()
    {
        base.OnFixedHandle();
        data.coyoteTime -= Time.fixedDeltaTime;
        if(rb.velocity.y > 0.1f){
            if(data.jumpRelease) {
                customGravity.gravityScale *= (1+data.gravAccel*Time.fixedDeltaTime);
            }    
            data.jumpPhase = 1;
        }else if(rb.velocity.y < -0.5f){
            if(IsOutOfCombat()) customGravity.gravityScale *= (1+data.gravAccel*Time.fixedDeltaTime);
            data.jumpPhase = 2;
            animationManager.SafeRemove(2, "JumpLoop");
            animationManager.SafeRemove(2, "AirFloatLoop");
            //Debug.Log("Added fall");
            animationManager.AddAnim(2, "Fall");
        }
        customGravity.gravityScale = Mathf.Min(customGravity.gravityScale,data.maxGravityScale);
    }

    public override void OnExit()
    {
        base.OnExit();
        data.ResetCoyoteTime();
        data.jumpsLeft = data.numberOfJumps;
        animationManager.SafeRemove(2, "AirFloatLoop");
        animationManager.SafeRemove(2, "Fall");
    }

    public override void UseJump()
    {
        base.UseJump();
        if (data.jumpsLeft > 0)
        {
            if (data.coyoteTime < 0.01f)
                data.jumpsLeft--;
            playerJump.CancelJump(data.JumpRoutine);
            data.JumpRoutine = data.StartCoroutine(loopJump());
        }
    }

    public override void UseMove()
    {
        base.UseMove();
        if (Mathf.Abs(data.movementInput.x) > 0.01f)
        {
            playerMove.RunCharacter(data.movementInput * data.hoverMult);
        }
    }

    public override void UseSlide()
    {
        base.UseSlide();
        playerMove.SlideCharacter();
    }

    private float getJumpPeakTime()
    {
        float res = data.jumpPower / data.customGravity.gravityScale / CustomGravity.globalGravity;
        return res;
    }

    private IEnumerator loopJump()
    {
        //Start
        playerJump.jumpStart(0);
        float jumpStartTime = (float)Time.timeAsDouble;

        yield return new WaitUntil(() => (float)Time.timeAsDouble - jumpStartTime >= getJumpPeakTime() - data.jumpAirMoveTime / 2 || data.jumpRelease);

        //Hang (Hover)
        animationManager.ForceAdd(2, "AirFloatLoop");
        playerJump.ToggleJumpHang(1.2f);
        float AirMoveTime = Time.time;
        yield return new WaitUntil(()=>(Time.time-AirMoveTime>data.jumpAirMoveTime) || data.groundChecker.isGrounded() );

        //Unhang (Unhover)
        playerJump.ToggleJumpHang(0.2f);
        animationManager.SafeRemove(2, "AirFloatLoop");

        data.jumpRelease = false;
        data.JumpRoutine = null;
    }
    
}