using UnityEngine;

public class GroundEntryState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        attackIndex = 1;
        animation_name = "Attack" + attackIndex;
        base.OnEnter(_stateMachine);
        animationManager.AddAnim(3, animation_name);
        //Attack
        duration = data.anims[animation_name].length/animator.speed;
        Debug.Log("Player Attack " + attackIndex + " Fired!");
    }

    public override void OnHandle()
    {
        base.OnHandle();
        if (time >= duration)
        {
            if (shouldCombo)
            {
                stateMachine.SetNextState(combatStates[ECombat.GroundCombo]);
            }
            else
            {
                stateMachine.SetNextStateToMain();
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        animationManager.RemoveAnim(3);
    }
}
