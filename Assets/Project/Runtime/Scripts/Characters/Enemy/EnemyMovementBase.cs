using UnityEngine;


public class EnemyMovementBase : State
{  
    #region Default Enemy Variables
    protected float duration;
    protected float sqrDistance;
    protected EnemyData data;
    protected EnemyController controller;
    protected PlayerData playerData;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector3 movementDirection;
    protected Transform GFX;
    public float chaseDistanceThreshold;
    public float movementSpeed; 
    #endregion

    #region Default Variables Known About Player
    protected Vector3 directionToPlayer;
    protected Rigidbody2D playerRigidBody;
    
    #endregion

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        controller = _stateMachine.controller as EnemyController;
        data = _stateMachine.GetComponent<EnemyData>();
        animator = data.animator;
        chaseDistanceThreshold = data.chaseDistanceThreshold;
        movementSpeed = data.movementSpeed;
        playerRigidBody = data.playerRigidBody;
        rb = data.rb;
        GFX = data.GFX;
    }

    public override void OnHandle()
    {
        base.OnHandle();
        BaseUpdate();
    }

    public void BaseUpdate()
    {   
        if(playerRigidBody == null) return;
        directionToPlayer = playerRigidBody.position-rb.position;
        sqrDistance = directionToPlayer.sqrMagnitude;
        //Debug.Log(playerRigidBody.position + " " + rb.position + " " + directionToPlayer + " " + sqrDistance);
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
