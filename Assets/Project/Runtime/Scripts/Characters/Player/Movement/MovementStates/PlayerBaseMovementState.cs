using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseMovementState : State
{
    protected PlayerData data;
    protected AnimationManager animationManager;
    protected PlayerMovementController movementController;
    protected Rigidbody2D rb;
    protected StateMachine<MeleeBaseState> combatMachine => data.combatMachine;
    protected CustomGravity customGravity;

    public virtual void Init(PlayerData data){
        this.data = data;
        animationManager = data.animationManager;
        movementController = data.movementController;
        rb = data.rb;
        customGravity = data.customGravity;
    }

    public virtual void UseMove(){
    }

    public virtual void UseSlide(){
        
    }

    public virtual void UseJump(){
        data.playerMove.CancelSlide();
    }

    protected bool IsOutOfCombat(){
        if(combatMachine == null || combatMachine.CurrentState == null) return false;
        return combatMachine.CurrentState.GetType() == typeof(CombatIdleState) || combatMachine.CurrentState.GetType() == typeof(MeleeEntryState);
    }
}