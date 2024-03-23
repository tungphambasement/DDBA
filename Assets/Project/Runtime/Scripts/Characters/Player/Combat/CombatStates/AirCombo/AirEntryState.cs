using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirEntryState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        attackIndex = 1;
        animation_name = "AirAttack" + attackIndex;
        base.OnEnter(_stateMachine);
        animationManager.AddAnim(3, animation_name);
        //Attack
        duration = data.anims[animation_name].length/animator.speed;
        Debug.Log("Player Air Attack " + attackIndex + " Fired!");
    }

    public override void OnHandle()
    {
        base.OnHandle();
        if (time >= duration)
        {
            if (shouldCombo)
            {
                stateMachine.SetNextState(combatStates[ECombat.AirCombo]);
            }
            else
            {
                stateMachine.SetNextStateToMain();
            }
        }
        rb.AddForce(new Vector3(0f,10f),ForceMode2D.Force);
    }

    public override void OnExit()
    {
        base.OnExit();
        animationManager.RemoveAnim(3);
    }
}
