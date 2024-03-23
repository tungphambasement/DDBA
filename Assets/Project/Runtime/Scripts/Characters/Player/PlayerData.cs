using System.Collections.Generic;
using Codice.Client.Common;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    #region ComponentsVariables
    [Header("Component Variables")]
    public Rigidbody2D rb;
    public Animator animator;
    public StateMachine combatMachine;
    public DamageFlash damageFlash;
    public HitCollider hitCollider;
    public GameObject Hiteffect;
    public GameObject playerBody;
    public GroundChecker groundChecker;
    public CustomGravity customGravity;
    public PlayerMovementController movementController;
    public PlayerCombatController combatController;
    public Transform GFX;
    public AnimationManager animationManager;
    public CapsuleCollider2D playerHitbox;
    #endregion

    [Space(20)]

    #region  Constant Data
    [Header("Constant Data")]
    [SerializeField] private ConstPlayerData constData;
    public float movementSpeed = 40f;
    public float velPow = 1f;
    public float runAccel = 2f;
    public float runDecel = 2f;
    public float jumpPower = 35f;
    public float jumpAirMoveTime = 0.8f;
    public float coyoteTime = 0.3f;
    public float defGrav = 1f;
    public float gravAccel = 2f;
    public int numberOfJumps = 1;
    #endregion

    [Space(20)]

    #region Movement Status
    [Header("Movement Status")]
    public Vector2 movementInput;
    public int jumpsLeft;
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
    [SerializeField] HealthBar healthBar;
    #endregion

    public float Difficulty = 0f;
    [SerializeField] Color fromColor, toColor;

    #region INumerable
    public Dictionary<string, AnimationClip> anims;
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
    }

    public void ResetJumpsCount(){
        jumpsLeft = numberOfJumps;
    }

    //Start is called before the first frame update
    void OnEnable()
    {
        Init();
        ResetJumpsCount();
    }

    void Update()
    {
        GFX.GetComponent<SpriteRenderer>().color = Color.Lerp(fromColor, toColor, Difficulty);
    }
}