using UnityEngine;

public class PlayerJump : PlayerBaseMovement
{
    private float jumpPower => data.jumpPower;
    
    public PlayerJump(PlayerData _data) : base(_data)
    {
        
    }

    public override void SelfInitialize()
    {
        base.SelfInitialize();
    }

    public override void SetUpConstants()
    {
        base.SetUpConstants();
    }

    public void jumpStart()
    {
        Debug.Log("Jump Started");
        float jumpForce = jumpPower - rb.velocity.y;
        rb.AddForce(new Vector2(0f,jumpForce),ForceMode2D.Impulse);
    }

    public void jumpExtend(){
        
    }
}
