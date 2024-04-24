using UnityEngine;


public class SlimeWander : SlimeMovementBase
{
    public SlimeWander(Slime_Data data) : base(data)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
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
        SlideMove(movementDirection);
        if (time >= duration)
        {                   
            data.idx = 1-data.idx;
        }
    }
}
