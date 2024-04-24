using UnityEngine;


public class EnemyMovementBase : State 
{  
    #region Default Enemy Variables
    protected float sqrDistance => data.sqrDistance;
    protected float duration;
    private EnemyData data;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector3 movementDirection;
    protected Transform GFX;
    public float chaseDistanceThreshold;
    public float movementSpeed; 
    #endregion

    public EnemyMovementBase(EnemyData data){
        this.data = data;
        animator = data.animator;
        chaseDistanceThreshold = data.chaseDistanceThreshold;
        movementSpeed = data.movementSpeed;
        rb = data.rb;
        GFX = data.GFX;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnHandle()
    {
        base.OnHandle();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void adjustFlipSprite(){
        if(movementDirection.x < 0){
            GFX.localScale = new Vector3(-1f,1f,1f);
        }else{
            GFX.localScale = new Vector3(1f,1f,1f);
        }
    }
}
