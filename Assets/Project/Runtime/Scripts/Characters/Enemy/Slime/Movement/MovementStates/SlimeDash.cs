using System.Collections.Generic;
using UnityEngine;

public class SlimeDash : SlimeMovementBase
{
    private float warmUp;
    private List<Controller> targetDamaged = new();
    private HitCollider hitCollider;
    public List<Collider> collidersToDamage;
    private float dashTime;


    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        BaseUpdate();
        movementDirection = directionToPlayer.normalized;
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
        if (warmUp > 0)
        {
            warmUp -= Time.fixedDeltaTime;
        }
        else if (dashTime > 0)
        {
            animator.SetBool("isDashing", true);
            TryDash(movementDirection);
            dashTime -= Time.fixedDeltaTime;
        }
        else
        {
            animator.SetBool("isDashing", false);
            stateMachine.SetNextState(new SlimeChase());
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
            if(collidersToDamage[i].CompareTag("LivingBody"))
                InflictDamage(collidersToDamage[i]);
        }
    }

    public virtual void InflictDamage(Collider targetCollider)
    {
        Controller target = targetCollider.GetComponentInParent<Controller>();
        if(target == null || targetDamaged.Contains(target)) return;
        TeamComponent hitTeamComponent = target.teamComponent;
        if (hitTeamComponent && hitTeamComponent.teamIndex == TeamIndex.Enemy)
        {
            target?.TakeDamage(10);
            Debug.Log("Enemy Has Taken: " + 10 + " Damage");
            targetDamaged.Add(target);
        }
    }
}
