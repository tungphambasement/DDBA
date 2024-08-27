using UnityEngine;

public class AirComboState : MeleeBaseState
{
    public override void OnEnter()
    {
        attackIndex = 2;
        animation_name = "AirAttack" + attackIndex;
        base.OnEnter();
        animationManager.AddAnim(3, animation_name);
        //Attack
        duration = data.anims[animation_name].length/attackSpeed;
        Debug.Log("Player Air Attack " + attackIndex + " Fired!");
    }

    public override void OnHandle()
    {
        base.OnHandle();
        rb.AddForce(new Vector2(0f,8f),ForceMode2D.Force);
    }

    public override void OnExit()
    {
        base.OnExit();
        animationManager.RemoveAnim(3);
    }
}
