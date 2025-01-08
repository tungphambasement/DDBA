public class SlimeDead : SlimeMovementBase
{
    public SlimeDead(Slime_Data data) : base(data)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animator.SetBool("isDead", true);
        data.controller.enabled = false;
    }
}
