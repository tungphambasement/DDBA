

using UnityEngine;

public class SlimeChase : SlimeMovementBase
{
    private float attackDistanceThreshold;

    public SlimeChase(Slime_Data data) : base(data)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        attackDistanceThreshold = data.attackDistanceThreshold;
        animator.SetBool("isChasing", true);
    }

    public override void OnFixedHandle()
    {
        base.OnFixedHandle();
        if (sqrDistance <= Mathf.Pow(chaseDistanceThreshold, 2))
        {
            movementDirection = data.directionToPlayer.normalized;
            adjustFlipSprite();
            SlideMove(movementDirection);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        animator.SetBool("isChasing", false);
    }
}
