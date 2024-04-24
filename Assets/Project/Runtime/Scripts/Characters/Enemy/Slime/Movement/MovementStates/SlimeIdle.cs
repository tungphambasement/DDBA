using UnityEngine;

public class SlimeIdle : SlimeMovementBase
{
    public SlimeIdle(Slime_Data data) : base(data)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
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
        if(time >= duration){
            data.idx = 1 -data.idx;
        }
    }
}
