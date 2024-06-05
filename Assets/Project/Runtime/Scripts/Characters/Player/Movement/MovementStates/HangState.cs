using UnityEngine;

public class HangState : PlayerBaseMovementState
{
    PlayerMove playerMove => data.playerMove;
    
    public override void Init(PlayerData data)
    {
        base.Init(data);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //Debug.Log("Entered Climbing");
        data.customGravity.gravityScale = 0;
        animationManager.ForceAdd(3, "WallHang");
        rb.velocity = Vector2.zero;
    }

    public override void OnFixedHandle()
    {
        base.OnFixedHandle();
        if (Mathf.Abs(data.movementInput.y) > 0.1f)
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
        

    }
}
