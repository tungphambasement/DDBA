using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    public Action _behaviors;

    public float time { get; private set; }
    public float fixedtime { get; private set; }
    public float latetime { get; private set; }

    public virtual void OnEnter()
    {
        time = fixedtime = latetime = 0;
    }

    public virtual void OnHandle()
    {
        time += Time.deltaTime;
    }

    public virtual void OnFixedHandle()
    {
        fixedtime += Time.deltaTime;
        _behaviors?.Invoke();
    }

    public virtual void OnLateHandle()
    {
        latetime += Time.deltaTime;
        
    }

    public virtual void OnExit()
    {

    }

    public virtual void OnFire()
    {

    }

}