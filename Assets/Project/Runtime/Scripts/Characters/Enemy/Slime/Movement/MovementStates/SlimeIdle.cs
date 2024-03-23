using UnityEngine;

public class SlimeIdle : SlimeMovementBase
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        randomizeIdle();
        movementDirection = Vector2.zero;
    }

    private void randomizeIdle(){
        duration = Random.Range(1.0f,2.5f);
    }

    public override void OnFixedHandle()
    {
        base.OnFixedHandle();
        duration -= Time.fixedDeltaTime;
        if(duration <= 0){
            if (sqrDistance < Mathf.Pow(chaseDistanceThreshold,2))
            {
                stateMachine.SetNextState(new SlimeChase());
            }
            else
            {
                revertToIdle();
            }
        }
    }
}
