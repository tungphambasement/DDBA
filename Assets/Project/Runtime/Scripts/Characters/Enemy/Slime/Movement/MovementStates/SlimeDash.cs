using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class SlimeDash : SlimeMovementBase
{
    private float warmUp;
    private List<Controller> targetDamaged = new();
    private HitCollider hitCollider;
    public List<Collider2D> collidersToDamage;
    private float dashTime;

    public SlimeDash(Slime_Data data) : base(data)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        movementDirection = data.directionToPlayer.normalized;
        adjustFlipSprite();
        animator.SetTrigger("Dash");
        warmUp = 0.4f;
        dashTime = data.dashTime;
        hitCollider = data.hitCollider;
        collidersToDamage = hitCollider.CollidersEntered = new();
    }

    public override void OnFixedHandle()
    {
        base.OnFixedHandle();
        if (time > warmUp)
        {
            animator.SetBool("isDashing", true);
            TryDash(movementDirection);
        }
        else if (time >= warmUp + dashTime)
        {
            animator.SetBool("isDashing", false);
            data.dashCD = data.defaultDashCD;
        }
    }

    void TryDash(Vector3 direction)
    {
        Attack();
        rb.AddForce(direction * movementSpeed * 3);
    }

    public virtual void Attack()
    {
        for (int i = 0; i < collidersToDamage.Count; i++)
        {
            Debug.Log("Collider is " + collidersToDamage[i].name);
            if (collidersToDamage[i].CompareTag("LivingBody"))
                InflictDamage(collidersToDamage[i]);
        }
    }

    public virtual void InflictDamage(Collider2D targetCollider)
    {
        Controller target = GameHandler.Instance.GetController(targetCollider);
        if (target == null || targetDamaged.Contains(target)) return;
        TeamComponent hitTeamComponent = target.teamComponent;
        if (hitTeamComponent && hitTeamComponent.teamIndex == TeamIndex.Enemy)
        {
            target?.TakeDamage(10);
            Debug.Log("Enemy Has Taken: " + 10 + " Damage");
            targetDamaged.Add(target);
        }
    }
}
