

using UnityEngine;

public class OnAirState : PlayerBaseMovementState{
    PlayerMove playerMove => data.playerMove;
    private bool isAirHover => data.AirHover;
    
    public override void Init(PlayerData data)
    {
        base.Init(data);
    }

    public override void OnEnter()
    {
        base.OnEnter();
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
        }else if(rb.velocity.y < -0.1f){
            if(IsOutOfCombat()) customGravity.gravityScale *= (1+data.gravAccel*Time.fixedDeltaTime);
            data.jumpPhase = 2;
            animationManager.SafeRemove(2, "AirFloatLoop");
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
    }

    public override void UseMove()
    {
        base.UseMove();
        if (Mathf.Abs(data.movementInput.x) > 0.01f)
        {
            float hoverMult = isAirHover ? 1.2f : 0.2f;
            playerMove.RunCharacter(data.movementInput * hoverMult);
        }
    }

    public override void UseSlide()
    {
        base.UseSlide();
        playerMove.SlideCharacter();
    }
}