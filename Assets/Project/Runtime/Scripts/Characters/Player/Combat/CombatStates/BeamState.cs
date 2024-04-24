using UnityEngine;

public class BeamState : MeleeBaseState
{
    private bool startingUp;

    public override void OnEnter()
    {
        animation_name = "CastStart";
        base.OnEnter();
        startingUp = true;
        animationManager.AddAnim(3, animation_name);
        data.isCasting = true;
        //Attack
        duration = data.anims[animation_name].length / attackSpeed;
        Debug.Log("Player Attack " + attackIndex + " Fired!");
    }

    public override void OnHandle()
    {
        base.OnHandle();
        if (startingUp)
        {
            if (time+Time.deltaTime >= duration)
            {
                animation_name = "CastLoop";
                animationManager.RemoveAnim(3);
                animationManager.AddAnim(3, animation_name);
                duration = data.anims[animation_name].length / attackSpeed;
                startingUp = false;
            }
        }else{
            
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        animationManager.RemoveAnim(3);
    }
}
