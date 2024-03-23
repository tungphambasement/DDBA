using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ECombat
{
    public const int CombatIdleState = 0;
    public const int MeleeEntryState = 1;
    public const int GroundEntry = 2;
    public const int GroundCombo = 3;
    public const int GroundFinisher = 4;
    public const int AirEntry = 5;
    public const int AirCombo = 6;
    public const int AirFinisher = 7;
}

public class PlayerCombatController : MonoBehaviour
{
    public List<State> combatStates;
    private PlayerData data;
    private StateMachine combatMachine;
    private PlayerMovementController movementController;
    public List<Collider> CollidersEntered;
    private AnimationManager animationManager;
    private bool weaponOut = false;

    public void SetController(PlayerData _data)
    {
        data = _data;
        SetDefaultVars();
    }

    private void SetDefaultVars()
    {
        combatMachine = data.combatMachine;
        movementController = data.movementController;
        animationManager = data.animationManager;
        SetupStates();
        SetupStateBehavior();
        combatMachine.mainStateType = combatStates[ECombat.CombatIdleState];
        combatMachine.SetNextStateToMain();
    }

    private void SetupStates()
    {
        combatStates = new List<State>
        {
            //Debug.Log(ECombat.GroundEntry + " " + combatStates.Capacity);
            new CombatIdleState(),
            new MeleeEntryState(),
            new GroundEntryState(),
            new GroundComboState(),
            new GroundFinisherState(),
            new AirEntryState(),
            new AirComboState(),
            new AirFinisherState()
        };
    }

    private void SetupStateBehavior()
    {
        foreach(MeleeBaseState meleeBaseState in combatStates){
            meleeBaseState.Init(data);
        }
        combatStates[ECombat.MeleeEntryState]._behaviors += ()=>movementController.UseMove();
        combatStates[ECombat.CombatIdleState]._behaviors += ()=>movementController.UseMove();
    }

    void FixedUpdate(){
        if(combatMachine.CurrentState != combatStates[0]){
            
        } 
    }

    public void OnFire()
    {
        combatMachine.CurrentState.OnFire();
    }
    
    Coroutine weaponCoroutine;

    public void WeaponOut()
    {
        if(!weaponOut){
            if(weaponCoroutine == null){ 
                weaponOut = true;
                weaponCoroutine = StartCoroutine(SwordOut());
            }
        }else{
            if(weaponCoroutine == null){
                weaponOut = false;
                weaponCoroutine = StartCoroutine(SwordIn());
            }
        }
    }

    private IEnumerator SwordOut(){
        animationManager.RemoveAnim(0);
        animationManager.AddAnim(0,"SwordOut");
        yield return new WaitForSeconds(0.5f);
        combatMachine.SetNextState(combatStates[ECombat.MeleeEntryState]);
        weaponCoroutine = null;
    }

    private IEnumerator SwordIn(){
        animationManager.RemoveAnim(0);
        animationManager.AddAnim(0,"SwordIn");
        yield return new WaitForSeconds(0.5f);
        combatMachine.SetNextState(combatStates[ECombat.CombatIdleState]);
        weaponCoroutine = null;
    }
}
