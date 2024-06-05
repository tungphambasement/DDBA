using System.Data.Common;
using UnityEngine;

public static class ESlime 
{
    public static int Idle = 0;
    public static int Wander = 1;
    public static int Chase = 2;
    public static int Dash = 3;
    public static int Hurt = 4;
    public static int Smash = 5;
}

public class SlimeMovementBase : EnemyMovementBase
{
    protected Slime_Data data;

    public SlimeMovementBase(Slime_Data data) : base(data)
    {
        this.data = data;
    }

    private float acceleration => data.acceleration;
    private float deceleration => data.deceleration;

    private float getAccel(float val){
        return (Mathf.Abs(val) > 0.01f) ? acceleration : deceleration;
    }

    public override void OnEnter()
    {
        base.OnEnter();
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
        if (data.dashCD > 0.0f) data.dashCD -= Time.fixedDeltaTime;
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
}
