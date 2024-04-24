using System.Collections;
using UnityEngine;

public class AirFinisherState : MeleeBaseState
{

    public override void OnEnter()
    {
        attackIndex = 3;
        animation_name = "AirAttack" + attackIndex;
        base.OnEnter();
        data.StartCoroutine(PlayAnimation());
        //Attack
        Debug.Log("Player Air Attack " + attackIndex + " Fired!");
        data.customGravity.gravityScale *= 2;
    }

    private IEnumerator PlayAnimation(){
        animationManager.AddAnim(3,animation_name);
        yield return new WaitForSeconds(data.anims[animation_name].length);
        animationManager.ForceAdd(3,animation_name+"Loop");
        yield return new WaitUntil(()=>data.groundChecker.isGrounded());
        //Debug.Log("Added " + animation_name + "End");
        animationManager.ForceAdd(3,animation_name+"End"); 
        yield return new WaitForSeconds(data.anims[animation_name+"End"].length);
        animationManager.RemoveAnim(3);
    }

    public override void OnHandle()
    {
        base.OnHandle();
    }

    public override void OnExit()
    {
        base.OnExit();
        data.customGravity.gravityScale /=2;
    }
}
