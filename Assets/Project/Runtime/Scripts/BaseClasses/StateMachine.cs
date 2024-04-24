using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : State
{
    public string customName;
    public T mainStateType;
    public T CurrentState { get; private set; }
    private T nextState;
    public Dictionary<T, List<Transition<T>>> stateTransitions;

    public void Init()
    {
        OnValidate();
        stateTransitions = new();
        CurrentState = mainStateType;
        SetNextStateToMain();
    }


    public void SetNextStateToMain()
    {
        nextState = mainStateType;
    }

    private void OnValidate()
    {
        if (mainStateType == null)
        {
        }
    }
    
    private void CheckTransitions(){
        if(CurrentState == null || !stateTransitions.ContainsKey(CurrentState)) return;
        List<Transition<T>> transitions = stateTransitions[CurrentState];
        if(transitions == null) return;
        foreach(Transition<T> transition in transitions){
            //Debug.Log(CurrentState + " " + transition.Condition + " " + transition.Condition()+  " "+ transition.nextState);
            if(transition.Condition()){
                nextState = transition.nextState;
                break;
            }
        }
    }

    public void SetNextState(T _newState)
    {
        if (_newState != null)
        {
            nextState = _newState;
        }
    }

    public void SetState(T _newState)
    {
        nextState = null;
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        CurrentState = _newState;
        CurrentState.OnEnter();
    }

    public void Handle()
    {
        if(nextState != null){
            SetState(nextState);
        }

        if (CurrentState != null)
            CurrentState.OnHandle();
    }

    public void FixedHandle()
    {
        CheckTransitions();

        if(nextState != null){
            SetState(nextState);
        }
        
        if (CurrentState != null)
            CurrentState.OnFixedHandle();
    }

    public void LateHandle()
    {
        if (CurrentState != null)
            CurrentState.OnLateHandle();
    }

}