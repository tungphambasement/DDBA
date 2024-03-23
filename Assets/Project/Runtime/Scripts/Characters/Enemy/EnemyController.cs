using System.Data.Common;
using UnityEngine;


public class EnemyController : Controller
{
    protected StateMachine stateMachine;
    protected Rigidbody2D rb;
    public EnemyData data;

    public int idx;

    public override void TakeDamage(float Damage)
    {
        throw new System.NotImplementedException();
    }

    public override void AddEffect(Effect effect)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    public virtual void Awake()
    {
        stateMachine = data.stateMachine;
        rb = data.rb;
        stateMachine.mainStateType = new IdleState();
        stateMachine.SetNextStateToMain();
    }

    public virtual void Start(){
    }

    public void FixedUpdate(){
        stateMachine.FixedHandle();
    }

    public void LateUpdate(){
        stateMachine.LateHandle();
    }
}
