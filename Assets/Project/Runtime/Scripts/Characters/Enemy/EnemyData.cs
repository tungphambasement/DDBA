using UnityEngine;


public class EnemyData : MonoBehaviour
{
    #region Components
    [Header("Components")]
    public Animator animator;
    public Rigidbody2D rb, playerRigidBody;
    public GameObject Hiteffect;
    public PlayerController playerController;
    public HitCollider hitCollider;
    public Transform GFX;
    #endregion

    #region Constants
    public float chaseDistanceThreshold = 1f, attackDistanceThreshold = 0.4f, dashTime = 1f, movementSpeed = 0.5f;
    public int idx;
    #endregion

    #region Status
    public float sqrDistance;
    public Vector2 directionToPlayer;

    #endregion

    public PlayerData playerData;

    void Start()
    {
        playerRigidBody = playerData.rb;
    }

    public void BaseUpdate()
    {
        if (playerRigidBody == null) return;
        directionToPlayer = playerRigidBody.position - rb.position;
        sqrDistance = directionToPlayer.sqrMagnitude;
    }

    private void FixedUpdate(){
        BaseUpdate();
    }

}