using UnityEngine;

public class GroundFinisherState : MeleeBaseState
{
    public override void OnEnter()
    {
        attackIndex = 3;
        animation_name = "Attack" + attackIndex;
        base.OnEnter();
        animationManager.AddAnim(3, animation_name);
        //Attack
        duration = data.anims[animation_name].length/attackSpeed;
        Debug.Log("Player Attack " + attackIndex + " Fired!");
        rb.AddForce(data.movementInput*30, ForceMode2D.Impulse);
    }

    public override void OnHandle()
    {
        base.OnHandle();
    }

    public override void OnExit()
    {
        base.OnExit();
        animationManager.RemoveAnim(3);
    }
}
