using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class MeleeEntryState : MeleeBaseState
{

    public override void OnEnter(StateMachine _stateMachine)
    {
        animation_name = "IdleCombat";
        base.OnEnter(_stateMachine);
        animationManager.RemoveAnim(0);
        
        animationManager.AddAnim(0, animation_name);
        data.customGravity.useGrav = true;
        duration = data.CombatCD;  
        _stateMachine.mainStateType = combatStates[ECombat.MeleeEntryState];
    }

    public override void OnHandle()
    {
        base.OnHandle();
    }

    public override void OnFire()
    {
        if(data.groundChecker.isGrounded()){
            stateMachine.SetNextState(combatStates[ECombat.GroundEntry]);
        }else{
            stateMachine.SetNextState(combatStates[ECombat.AirEntry]);
        }
    }
}
