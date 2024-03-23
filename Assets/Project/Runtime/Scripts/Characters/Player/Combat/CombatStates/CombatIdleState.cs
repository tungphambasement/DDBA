using UnityEngine;

public class CombatIdleState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        animation_name = "Idle";
        base.OnEnter(_stateMachine);
        animationManager.RemoveAnim(0);
        animationManager.AddAnim(0, animation_name);
        _stateMachine.mainStateType = combatStates[ECombat.CombatIdleState];
    }

    public override void OnHandle()
    {
        base.OnHandle();
    }
}