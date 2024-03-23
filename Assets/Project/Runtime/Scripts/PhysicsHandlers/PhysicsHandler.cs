using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct PhysicsState{
    public float x;
    public float v;
}

struct DerivedPhysicsState{
    public float dx;
    public float dv;
}

[System.Serializable]
public class PhysicsHandler : MonoBehaviour
{
    [Header("RK4 Integration Method")]
    [SerializeField] float k = 15.0f;
    [SerializeField] float b = 0.1f; 
    //t stands for time, dt stands for derived time
    DerivedPhysicsState Evaluate(PhysicsState _initial, DerivedPhysicsState _derivedinitial, double t, float dt){
        PhysicsState momentum;
        momentum.x = _initial.x + _derivedinitial.dx*dt;
        momentum.v = _initial.v + _derivedinitial.dv*dt;
        DerivedPhysicsState derivedMomentum; 
        derivedMomentum.dx = momentum.v;
        derivedMomentum.dv = Acceleration(momentum,t+dt);

        return derivedMomentum;
    }

    float Acceleration(PhysicsState state, double t){
        return -k * state.x - b * state.v;
    }

    void Integrate(PhysicsState state, double t, float dt )
    {
        DerivedPhysicsState a,b,c,d;
        a.dx = a.dv = 0;
        a = Evaluate(state,a,t, 0.0f);
        b = Evaluate(state,a,t,dt*0.5f);
        c = Evaluate(state,b,t,dt*0.5f);
        d = Evaluate(state,c,t,dt);
        float dxdt = 1.0f / 6.0f * 
            (a.dx + 2.0f * ( b.dx + c.dx ) + d.dx );
        
        float dvdt = 1.0f / 6.0f * 
            (a.dv + 2.0f * ( b.dv + c.dv ) + d.dv );

        state.x = state.x + dxdt * dt;
        state.v = state.v + dvdt * dt;
    }
}
