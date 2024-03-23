using System;
using System.Collections.Generic;
using UnityEngine;

public class Transition
{
    public Func<bool> Condition { get;} 
    public State nextState;

    public Transition(Func<bool> condition, State _nextState){
        Condition = condition;
        nextState = _nextState;
    }

}