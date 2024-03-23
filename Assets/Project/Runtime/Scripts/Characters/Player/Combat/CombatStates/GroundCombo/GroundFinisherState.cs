using UnityEngine;

public class GroundFinisherState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        attackIndex = 3;
        animation_name = "Attack" + attackIndex;
        base.OnEnter(_stateMachine);
        animationManager.AddAnim(3, animation_name);
        //Attack
        attackIndex = 3;
        duration = data.anims[animation_name].length/animator.speed;
        Debug.Log("Player Attack " + attackIndex + " Fired!");
    }

    public override void OnHandle()
    {
        base.OnHandle();
        if (time >= duration)
        {
             stateMachine.SetNextStateToMain();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        animationManager.RemoveAnim(3);
    }
}
