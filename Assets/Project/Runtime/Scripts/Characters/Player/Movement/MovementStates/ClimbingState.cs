using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ClimbingState : PlayerBaseMovementState
{
    PlayerMove playerMove => data.playerMove;

    PlayerDash playerDash => data.playerDash;

    public override void Init(PlayerData data)
    {
        base.Init(data);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //Debug.Log("Entered Climbing");
        data.multVel(0f);
        data.customGravity.gravityScale = 0;
        animationManager.ForceAdd(3, "WallCling");
        playerDash.StopDashCharacter();
        data.jumpPower += 20;
    }

    private float lastmovementInput = 0f;

    public override void OnFixedHandle()
    {
        base.OnFixedHandle();
        if(Mathf.Abs(data.movementInput.y) > 0.1f){
            playerMove.ClimbCharacter();
        }else{
            playerMove.StopClimbCharacter();
            animationManager.AddAnim(3, "WallCling");
        }
        lastmovementInput = data.movementInput.y;
    }

    public override void OnExit()
    {
        base.OnExit();
        playerMove.StopClimbCharacter();
        animationManager.RemoveAnim(3); 
        customGravity.gravityScale = data.defGrav;
        data.jumpPower -= 20;
        data.velocityMult = 1;
    }

    public override void UseSlide()
    {
    }
}