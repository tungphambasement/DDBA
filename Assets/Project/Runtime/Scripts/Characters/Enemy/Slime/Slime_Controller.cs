using System.Collections.Generic;
using UnityEngine;

public class SlimeController : EnemyController, IMovable
{   
    new Slime_Data data;
    public StateMachine<SlimeMovementBase> stateMachine;
    public List<SlimeMovementBase> slimeStates;


    // Start is called before the first frame update
    public override void Awake(){
        base.Awake();
        SetupStates();
        stateMachine.mainStateType = slimeStates[ESlime.Idle];
        stateMachine.SetNextStateToMain();
    }

    public void moveSelf(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction);
    }

    public override void Start(){
        base.Start();
        data.currentHealth = data.maxHealth;
        data.dashCD = 0;
    }
    
    public override void TakeDamage(float Damage){
        data.currentHealth -= Damage;
        stateMachine.SetNextState(slimeStates[ESlime.Hurt]);
    }   

    public void Update()
    {
        stateMachine.Handle();
    }

     private void SetupStates()
    {
        slimeStates = new List<SlimeMovementBase>
        {
            new SlimeIdle(data),
            new SlimeWander(data),
            new SlimeChase(data),
            new SlimeDash(data),
            new SlimeHurt(data),
            new SlimeSmash(data),
        };
        SetupTransitions();
    }

    private bool isChasing(){
        return data.sqrDistance <= Mathf.Pow(data.chaseDistanceThreshold,2);
    }

    private bool isDashing(){
        return data.sqrDistance <= Mathf.Pow(data.attackDistanceThreshold,2) && data.dashCD <= 0;
    }

    private bool isDead(){
        return data.currentHealth <= 0;
    }

    private void SetupTransitions(){
        //Idle
        stateMachine.stateTransitions.Add(
            slimeStates[ESlime.Idle],new(){
                new(()=>data.idx == 1, slimeStates[ESlime.Wander]),
                new(isChasing, slimeStates[ESlime.Chase])
                }
        );
        
        //Wander
        stateMachine.stateTransitions.Add(
            slimeStates[ESlime.Wander],new(){
                new(()=>data.idx == 0, slimeStates[ESlime.Idle]),
                new(isChasing, slimeStates[ESlime.Chase])
                }
        );

        //Chase
        stateMachine.stateTransitions.Add(
            slimeStates[ESlime.Chase],new(){
                new(()=>!isChasing(), slimeStates[ESlime.Idle]),
                new(isDashing, slimeStates[ESlime.Dash])
                }
        );

        //Dash
        stateMachine.stateTransitions.Add(
            slimeStates[ESlime.Chase],new(){
                new(()=>!isDashing(), slimeStates[ESlime.Chase]),
                }
        );

        //Hurt
        stateMachine.stateTransitions.Add(
            slimeStates[ESlime.Hurt],new(){
                new(()=>!isDead(), slimeStates[ESlime.Idle]),
                }
        );
    }   
}
