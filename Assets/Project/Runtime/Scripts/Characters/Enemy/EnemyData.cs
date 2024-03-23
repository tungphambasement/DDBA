using UnityEngine;


public class EnemyData : MonoBehaviour
{
    #region Components
    [Header("Components")]
    public Animator animator;
    public Rigidbody2D rb, playerRigidBody;
    public GameObject Hiteffect;
    public PlayerController playerController;
    public StateMachine stateMachine;
    public HitCollider hitCollider;
    public Transform GFX;
    #endregion

    #region Constants
    public float chaseDistanceThreshold = 1f, attackDistanceThreshold = 0.4f, dashTime = 1f, movementSpeed = 0.5f;
    public int idx;
    #endregion

    public PlayerData playerData;

    void Start(){
        playerRigidBody = playerData.rb;
    }
}