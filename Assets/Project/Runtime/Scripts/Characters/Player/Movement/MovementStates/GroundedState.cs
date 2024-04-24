using System;
using UnityEngine;

public class GroundedState : PlayerBaseMovementState
{
    PlayerMove playerMove => data.playerMove;

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
    }
}