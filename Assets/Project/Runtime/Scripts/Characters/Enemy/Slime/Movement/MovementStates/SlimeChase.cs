

using UnityEngine;

public class SlimeChase : SlimeMovementBase
{ 
    private float attackDistanceThreshold;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        attackDistanceThreshold = data.attackDistanceThreshold;
        animator.SetBool("isChasing",true);
    }

    public override void OnFixedHandle()
    {   
        base.OnFixedHandle();
        BaseUpdate();
        if (sqrDistance < Mathf.Pow(chaseDistanceThreshold,2))
        {
            movementDirection = directionToPlayer.normalized;
            adjustFlipSprite();
            if(sqrDistance < attackDistanceThreshold && slimeController.dashCD <= 0){
                slimeController.dashCD = slimeController.defaultDashCD;
                stateMachine.SetNextState(new SlimeDash());
            }else{
                SlideMove(movementDirection);
            }
        }
        else
        {
            revertToIdle();
        }
        
    }

    public override void OnExit()
    {
        base.OnExit();
        animator.SetBool("isChasing",false);
    }
}
