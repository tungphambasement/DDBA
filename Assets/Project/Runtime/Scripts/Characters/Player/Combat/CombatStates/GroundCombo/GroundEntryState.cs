using UnityEngine;

public class GroundEntryState : MeleeBaseState
{
    public override void OnEnter()
    {
        attackIndex = 1;
        animation_name = "Attack" + attackIndex;
        base.OnEnter();
        animationManager.AddAnim(3, animation_name);
        //Attack
        duration = data.anims[animation_name].length/attackSpeed;
        Debug.Log("Player Attack " + attackIndex + " Fired!");
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
