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

    public void jumpStart(float deg = 0)
    {
        //Debug.Log("Jump Started");
        animationManager.ForceAdd(2, "JumpLoop");
        customGravity.gravityScale = data.defGrav;

        float jumpForce = jumpPower - rb.velocity.y;
        float verticalForce = jumpForce * Mathf.Cos(deg * Mathf.Deg2Rad);
        float horizontalForce = jumpForce * Mathf.Sin(deg * Mathf.Deg2Rad) * _GFX.localScale.x * -1;
        rb.AddForce(new Vector2(horizontalForce,verticalForce),ForceMode2D.Impulse);
    }

    public void ToggleJumpHang(float hoverMult){
        if(!data.AirHover){
            data.hoverMult = hoverMult;
            data.AirHover = true; 
            data.gravAccel /= 2;
        }else{
            data.hoverMult = 0.2f;
            data.AirHover = false;
            data.gravAccel *= 2;
        }
    }

    public void CancelJump(Coroutine JumpRoutine){
        if(JumpRoutine == null) return;
        if(data.AirHover){
            ToggleJumpHang(0.2f);
        }else{
            data.canFlip = true;
        }
        //Debug.Log("Cancelling Jump");
        data.jumpRelease = false;
        data.jumpPhase = 0; 
        animationManager.RemoveAnim(2);
        data.StopCoroutine(JumpRoutine);
        JumpRoutine = null;
    }

}
