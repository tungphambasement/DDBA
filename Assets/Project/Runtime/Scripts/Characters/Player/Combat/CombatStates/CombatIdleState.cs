using UnityEngine;

public class CombatIdleState : MeleeBaseState
{
    public override void OnEnter()
    {
        animation_name = "Idle";
        base.OnEnter();
        animationManager.RemoveAnim(0);
        animationManager.AddAnim(0, animation_name);
    }

    public override void OnHandle()
    {
        base.OnHandle();
    }
}