using UnityEngine;


public class SlimeMovementBase : EnemyMovementBase
{
    protected SlimeController slimeController;
    private float acceleration => slimeController.acceleration;
    private float deceleration => slimeController.deceleration;

    private float getAccel(float val){
        return (Mathf.Abs(val) > 0.01f) ? acceleration : deceleration;
    }

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        slimeController = controller as SlimeController;
        //data = base.data as Slime_Data;
        rb = data.rb;
    }

    public override void OnHandle()
    {
        base.OnHandle();
        adjustFlipSprite();
    }
    
    public override void OnFixedHandle()
    {
        base.OnFixedHandle();
        if (slimeController.dashCD > 0.0f) slimeController.dashCD -= Time.fixedDeltaTime;
        animator.SetFloat("Speed",rb.velocity.magnitude);
    }

    public void SlideMove(Vector2 direction)
    {
        Vector2 targetSpeed = movementSpeed * direction;
        Vector2 speedDif = targetSpeed - rb.velocity;
        speedDif.y = 0;
        Vector2 accelRate = new Vector2(getAccel(targetSpeed.x),0f);
        Vector2 curSpeed = new Vector2(speedDif.x*accelRate.x,0f);
        //Debug.Log(accelRate + " " + speedDif + " " + curSpeed); 
        rb.AddForce(curSpeed);
    }
    
    public void revertToIdle(){
        data.idx = 1-data.idx;
        switch (data.idx)
        {   
            case 0:
                stateMachine.SetNextState(new SlimeIdle());
                break;
            case 1:
                stateMachine.SetNextState(new SlimeWander());
                break;
        }
    }
}
