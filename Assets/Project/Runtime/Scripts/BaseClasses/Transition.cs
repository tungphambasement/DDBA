using System;
using System.Collections.Generic;
using UnityEngine;

public class Transition<T> where T : State
{
    public Func<bool> Condition { get;} 
    public T nextState;

    public Transition(Func<bool> condition, T _nextState){
        Condition = condition;
        nextState = _nextState;
    }

}