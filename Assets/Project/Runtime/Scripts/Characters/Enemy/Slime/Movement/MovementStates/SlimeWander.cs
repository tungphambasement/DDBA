using UnityEngine;


public class SlimeWander : SlimeMovementBase
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        randomizeWander();
    }

    private void randomizeWander(){
        movementDirection = new Vector3(Random.Range(-1.0f,1.0f),0f,Random.Range(-1.0f,1.0f)); 
        duration = Random.Range(1.0f,2.5f);
        movementDirection.Normalize();
    }

    public override void OnFixedHandle()
    {
        base.OnFixedHandle();
        duration -= Time.fixedDeltaTime;
        if (sqrDistance < Mathf.Pow(chaseDistanceThreshold,2))
        {
            stateMachine.SetNextState(new SlimeChase());
        }
        if (duration <= 0)
        {                   
            revertToIdle();
            
        }
        SlideMove(movementDirection);
        
    }
}
