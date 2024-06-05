using System.Collections.Generic;
using System.Data.Common;
using Codice.Client.Common;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerData : MonoBehaviour
{
    #region ComponentsVariables
    [Header("Component Variables")]
    public Rigidbody2D rb;
    public Animator animator;
    public StateMachine<MeleeBaseState> combatMachine = new();
    public StateMachine<PlayerBaseMovementState> movementMachine = new();
    public DamageFlash damageFlash;
    public HitCollider hitCollider;
    public GameObject Hiteffect;
    public GameObject playerBody;
    public GroundChecker groundChecker;
    public CustomGravity customGravity;
    public PlayerMovementController movementController;
    public PlayerCombatController combatController;
    public PlayerInputController inputController;
    public Transform GFX, ClimbUpper, ClimbLower;
    public AnimationManager animationManager;
    public CapsuleCollider2D playerHitbox;
    public SpriteRenderer spriteRenderer;
    public CollisionListener collisionListener;
    #endregion

    [Space(20)]

    #region  Constant Data
    [Header("Constant Data")]
    [SerializeField] private ConstPlayerData constData;
    #region  Run
    public float movementSpeed = 40f;
    public float velPow = 1f;
    public float runAccel = 2f;
    public float runDecel = 2f;
    #endregion

    #region Jump
    public float jumpPower = 35f;
    public float jumpAirMoveTime = 0.8f;
    public float coyoteTime = 0.3f;
    public float hoverMult;
    public float jumpDegree = 0;
    public int numberOfJumps = 1;
    public float maxGravityScale = 4f;
    #endregion

    #region Dash
    public float dashTime;
    public float afterImageDistance;
    #endregion
    
    #region Crouch
    #endregion

    public float defGrav = 1f;
    public float gravAccel = 2f;
    public float climbSpeed = 5f;
    #endregion

    [Space(20)]

    #region Movement Status
    [Header("Movement Status")]
    public Vector2 movementInput;
    public int jumpPhase;
    public int jumpsLeft;
    public bool jumpRelease, AirHover;
    public bool canMove = true, canJump = true, canFlip = true, canAttack = true;
    public bool isCrouching = false, isDashing = false, isSliding = false;
    public bool isAccelGrav = true;
    public bool topHit = false, upperHit = false, midHit = false, lowerHit = false, bottomHit = false;
    public bool shouldCombo, isCasting;
    public Coroutine JumpRoutine;
    #endregion

    [Space(20)]

    #region CombatConsts
    [Header("Combat Constants")]
    public float CombatCD = 5f;
    #endregion

    [Space(20)]

    #region  Player Status
    [Header("Player Status")]
    public float maxHealth;
    public float inCombatCD;
    #endregion

    [Space(20)]

    #region Foreign Variables
    [Header("Foreign Variables")]
    public HealthBar healthBar;
    public DashTrail dashTrail;
    public GameObject afterImagePrefabs;
    #endregion

    public float Difficulty = 0f;
    [SerializeField] Color fromColor, toColor;

    #region INumerable
    [Header("IEnumerable")]
    public Transform AfterImages;
    public Dictionary<string, AnimationClip> anims;
    #endregion

    #region Movement Capabilities
    public PlayerMove playerMove { get; private set;}
    public PlayerJump playerJump { get; private set;}
    public PlayerDash playerDash { get; private set;}
    #endregion

    private void Init()
    {
        movementSpeed = constData.movementSpeed;
        velPow = constData.velPow;
        runAccel = constData.runAccel;
        runDecel = constData.runDecel;
        jumpPower = constData.jumpPower;
        jumpAirMoveTime = constData.jumpAirMoveTime;
        coyoteTime = constData.coyoteTime;
        defGrav = constData.defGrav;
        gravAccel = constData.gravAccel;
        numberOfJumps = constData.numberOfJumps;

        playerMove = new PlayerMove(this);
        playerJump = new PlayerJump(this);
        playerDash = new PlayerDash(this);
    }

    public void ResetJumpsCount(){
        jumpsLeft = numberOfJumps;
    }
    
    public void ResetCoyoteTime(){
        coyoteTime = constData.coyoteTime;
    }

    public bool isWallAhead(){
        return movementController.isWallAhead();
    }

    //Start is called before the first frame update
    void OnEnable()
    {
        Init();
        ResetJumpsCount();
        animator.SetFloat("AttackSpeed", 1.5f);
    }

    void Update()
    {
    }
}