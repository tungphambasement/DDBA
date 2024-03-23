using UnityEngine;

public class SlimeController : EnemyController, IMovable
{   
    readonly new Slime_Data data;

    public float currentHealth, maxHealth = 30;

    public float defaultDashCD = 4, dashCD;

    public float acceleration = 2f, deceleration = 2f;


    // Start is called before the first frame update
    public override void Awake(){
        base.Awake();
    }

    public void moveSelf(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction);
    }

    public override void Start(){
        base.Start();
        currentHealth = maxHealth;
        dashCD = 0;
    }
    
    public override void TakeDamage(float Damage){
        currentHealth -= Damage;
        stateMachine.SetNextState(new SlimeHurt());
    }   

    public void Update()
    {
        //Debug.Log(stateMachine.CurrentState);
        if (stateMachine.CurrentState.GetType() == typeof(IdleState))
        {
            stateMachine.SetNextState(new SlimeIdle());
        }
        stateMachine.Handle();
    }
}
