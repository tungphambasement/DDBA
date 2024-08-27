using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    public const int Beam = 8;
}

public class PlayerCombatController : MonoBehaviour
{
    public List<MeleeBaseState> combatStates;
    private PlayerData data;
    private StateMachine<MeleeBaseState> combatMachine;
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
        combatMachine.Init();
        movementController = data.movementController;
        animationManager = data.animationManager;
        SetupStates();
        SetupStateBehavior();
        combatMachine.mainStateType = combatStates[ECombat.CombatIdleState];
        combatMachine.SetNextStateToMain();
    }

    private void SetupStates()
    {
        combatStates = new List<MeleeBaseState>
        {
            new CombatIdleState(),
            new MeleeEntryState(),
            new GroundEntryState(),
            new GroundComboState(),
            new GroundFinisherState(),
            new AirEntryState(),
            new AirComboState(),
            new AirFinisherState(),
            new BeamState()
        };
        SetupTransitions();
    }

    private void SetupTransitions(){
        //Melee Entry
        combatMachine.stateTransitions.Add(
            combatStates[ECombat.MeleeEntryState],new(){
                new(()=>data.shouldCombo&&data.groundChecker.isGrounded(), combatStates[ECombat.GroundEntry]),
                new(()=>data.shouldCombo&&!data.groundChecker.isGrounded(), combatStates[ECombat.AirEntry])
                }
        );

        //Ground Entry
        combatMachine.stateTransitions.Add(
            combatStates[ECombat.GroundEntry],new(){
                new(()=>data.shouldCombo, combatStates[ECombat.GroundCombo]),
                new(()=>combatStates[ECombat.GroundEntry].time >= combatStates[ECombat.GroundEntry].duration, combatStates[ECombat.MeleeEntryState])
                }
        );
        
        //Ground Combo
        combatMachine.stateTransitions.Add(
            combatStates[ECombat.GroundCombo],new(){
                new(()=>data.shouldCombo, combatStates[ECombat.GroundFinisher]),
                new(()=>combatStates[ECombat.GroundCombo].time >= combatStates[ECombat.GroundCombo].duration, combatStates[ECombat.MeleeEntryState])
                }
        );

        //Ground Finisher
        combatMachine.stateTransitions.Add(
            combatStates[ECombat.GroundFinisher],new(){
                new(()=>combatStates[ECombat.GroundFinisher].time >= combatStates[ECombat.GroundFinisher].duration, combatStates[ECombat.MeleeEntryState])
                }
        );

        //Air Entry
        combatMachine.stateTransitions.Add(
            combatStates[ECombat.AirEntry],new(){
                new(()=>data.shouldCombo, combatStates[ECombat.AirCombo]),
                new(()=>combatStates[ECombat.AirEntry].time >= combatStates[ECombat.AirEntry].duration, combatStates[ECombat.MeleeEntryState])
                }
        );
        
        //Air Combo
        combatMachine.stateTransitions.Add(
            combatStates[ECombat.AirCombo],new(){
                new(()=>data.shouldCombo, combatStates[ECombat.AirFinisher]),
                new(()=>combatStates[ECombat.AirCombo].time >= combatStates[ECombat.AirCombo].duration, combatStates[ECombat.MeleeEntryState])
                }
        );

        //Air Finisher
        combatMachine.stateTransitions.Add(
            combatStates[ECombat.AirFinisher],new(){
                new(data.groundChecker.isGrounded, combatStates[ECombat.MeleeEntryState])
                }
        );

        //Beam Cast
        combatMachine.stateTransitions.Add(
            combatStates[ECombat.Beam],new(){
                new(()=>!data.isCasting, combatStates[ECombat.MeleeEntryState])
                }
        );
    }   

    private void SetupStateBehavior()
    {
        foreach (MeleeBaseState meleeBaseState in combatStates)
        {
            meleeBaseState.Init(data);
        }
        combatStates[ECombat.MeleeEntryState]._behaviors += () => movementController.UseMove();
        combatStates[ECombat.CombatIdleState]._behaviors += () => movementController.UseMove();
    }

    private void Update()
    {
        combatMachine.Handle();
    }

    void FixedUpdate()
    {
        combatMachine.FixedHandle();
        //Debug.Log("Should Combo " + data.shouldCombo);
        //Debug.Log("Time of Current State: " + combatMachine.CurrentState.time + " Duration: " + combatMachine.CurrentState.duration);
    }

    public void OnFire()
    {
        combatMachine.CurrentState.OnFire();
    }

    public void OnCast_Performed()
    {
        combatMachine.SetNextState(combatStates[ECombat.Beam]);
        data.isCasting = true;
    }

    public void OnCast_Released()
    {
        combatMachine.CurrentState.OnCast_Released();
    }

    Coroutine weaponCoroutine;

    private void ToggleIdle(bool state)
    {
        data.canAttack = data.canFlip = data.canJump = data.canMove = !state;
    }

    public void WeaponOut()
    {
        if (!weaponOut)
        {
            if (weaponCoroutine == null)
            {
                weaponOut = true;
                ToggleIdle(true);
                weaponCoroutine = StartCoroutine(SwordOut());
                combatMachine.mainStateType = combatStates[ECombat.MeleeEntryState];
            }
        }
        else
        {
            if (weaponCoroutine == null)
            {
                weaponOut = false;
                ToggleIdle(true);
                weaponCoroutine = StartCoroutine(SwordIn());
                combatMachine.mainStateType = combatStates[ECombat.CombatIdleState];
            }
        }
    }

    private IEnumerator SwordOut()
    {
        animationManager.RemoveAnim(0);
        animationManager.AddAnim(0, "SwordOut");
        yield return new WaitForSeconds(data.anims["SwordOut"].length);
        combatMachine.SetNextState(combatStates[ECombat.MeleeEntryState]);
        ToggleIdle(false);
        weaponCoroutine = null;
    }

    private IEnumerator SwordIn()
    {
        animationManager.RemoveAnim(0);
        animationManager.AddAnim(0, "SwordIn");
        yield return new WaitForSeconds(data.anims["SwordIn"].length);
        combatMachine.SetNextState(combatStates[ECombat.CombatIdleState]);
        ToggleIdle(false);
        weaponCoroutine = null;
    }
}
