using System.Data.Common;
using UnityEngine;


public class EnemyController : Controller
{
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
        rb = data.rb;
    }

    public virtual void Start(){
    }
}
