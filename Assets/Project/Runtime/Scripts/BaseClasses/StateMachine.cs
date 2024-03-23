using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public string customName;
    public State mainStateType;
    public State CurrentState { get; private set; }
    private State nextState;
    public Controller controller;

    private void Awake()
    {
        SetController(GetComponent<Controller>());
    
        OnValidate();
        CurrentState = mainStateType;
        SetNextStateToMain();
    }

    private void SetController(Controller _controller)
    {
        controller = _controller;
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

    public void SetNextState(State _newState)
    {
        if (_newState != null)
        {
            nextState = _newState;
        }
    }

    public void SetState(State _newState)
    {
        nextState = null;
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        CurrentState = _newState;
        CurrentState.OnEnter(this);
    }

    private void CheckTransition(){
        //Debug.Log(controller.transform.name + " " + customName + " " + CurrentState);
        if(CurrentState?._transitions == null) return;
        foreach(Transition transition in CurrentState._transitions){
            if(transition.Condition()){
                nextState = transition.nextState;
            }
        }
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
        if (CurrentState != null)
            CurrentState.OnFixedHandle();
    }

    public void LateHandle()
    {
        if (CurrentState != null)
            CurrentState.OnLateHandle();
    }
}