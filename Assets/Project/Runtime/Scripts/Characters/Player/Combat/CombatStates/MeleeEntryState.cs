

public class MeleeEntryState : MeleeBaseState
{

    public override void OnEnter()
    {
        animation_name = "IdleCombat";
        base.OnEnter();
        animationManager.RemoveAnim(0);
        
        animationManager.AddAnim(0, animation_name);
        duration = data.CombatCD;  
    }

    public override void OnHandle()
    {
        base.OnHandle();
    }

    public override void OnFire()
    {
        data.shouldCombo = true;
    }
}
