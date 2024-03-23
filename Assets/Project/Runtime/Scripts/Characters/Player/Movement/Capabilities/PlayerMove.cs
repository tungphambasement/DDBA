using UnityEngine;

public class PlayerMove : PlayerBaseMovement
{
    private float movementSpeed => data.movementSpeed;
    private float velPow => data.velPow;
    private float acceleration => data.runAccel;
    private float deceleration => data.runDecel;
    private bool canMove = true;
    private float frictionConst = 0.5f;
    private bool isCrouching;
    
    public PlayerMove(PlayerData _data) : base(_data)
    {
        data = _data;
        SelfInitialize();
        SetUpConstants();
    }

    public override void SelfInitialize()
    {
        base.SelfInitialize();
        isCrouching = false;
    }

    public override void SetUpConstants()
    {
        base.SetUpConstants();
    }

    public void SetMovabibility(bool flag){
        canMove = flag;
    }
    
    private float getAccel(float val){
        return (Mathf.Abs(val) > 0.01f) ? acceleration : deceleration;
    }
    
    private float getFriction(float curVel, float curInput){
        if(Mathf.Abs(curInput) > 0.01f) return 0f;
        float res = Mathf.Min(Mathf.Abs(curVel),Mathf.Abs(frictionConst));
        res *= Mathf.Sign(curVel);
        return -res;
    }

    public void moveCharacter(Vector2 direction){
        //Add Force when player walks
        Vector2 targetSpeed = movementSpeed * direction;
        Vector2 speedDif = targetSpeed - rb.velocity;
        speedDif.y = 0;
        Vector2 accelRate = new Vector2(getAccel(targetSpeed.x),0f);
        Vector2 curSpeed = new Vector2(speedDif.x*accelRate.x,0f);
        //Debug.Log(accelRate + " " + speedDif + " " + curSpeed); 
        rb.AddForce(curSpeed);
        animator.speed = Mathf.Max(1f,2*Mathf.Abs(rb.velocity.x)/(Mathf.Abs(rb.velocity.x)+10));
    }

    public void brakeMovement(Vector2 direction){
        //Friction handling
        Vector2 frictionForce = new Vector2(getFriction(rb.velocity.x,direction.x),0f);
        rb.AddForce(frictionForce, ForceMode2D.Impulse);
    }

    public void crouchCharacter(){
        if(!isCrouching){
            Debug.Log("Is Crouching");
            isCrouching = true;
            data.animationManager.AddAnim(2,"Crouch");
            data.movementSpeed /= 3f;
            data.runAccel *= 2f;
        }else{
            Debug.Log("Uncrouched");
            isCrouching = false;
            data.animationManager.RemoveAnim(2);
            data.movementSpeed *= 3f;
            data.runAccel /= 2f;
        }
    }
}


