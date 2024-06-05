using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MeleeBaseState : State
{
    #region Local Var
    public float duration;
    protected List<Collider2D> colliderToDamage;
    private List<Controller> targetDamaged;
    protected List<MeleeBaseState> combatStates;
    protected float attackIndex, attackSpeed;
    // Input buffer Timer
    private float AttackPressedTimer;
    private bool allowMove = false;
    protected string animation_name;
    #endregion

    #region Component Variables
    protected PlayerData data;
    protected PlayerController playerController;
    protected Animator animator;
    protected HitCollider hitCollider;
    protected Rigidbody2D rb;
    protected AnimationManager animationManager;
    protected PlayerMovementController movementController => data.movementController;
    #endregion

    // The Hit Effect to Spawn on the afflicted Enemy
    private GameObject HitEffectPrefab;

    public void Init(PlayerData _data)
    {
        data = _data;
        rb = data.rb;
        combatStates = data.combatController.combatStates;
        animator = data.animator;
        hitCollider = data.hitCollider;
        playerController = data.GetComponent<PlayerController>();
        HitEffectPrefab = data.Hiteffect;
        animationManager = data.animationManager;
        
    }

    

    public override void OnEnter()
    {
        base.OnEnter();
        AttackPressedTimer = 0;
        data.shouldCombo = false;
        colliderToDamage = hitCollider.CollidersEntered = new();
        targetDamaged = new();
        data.customGravity.gravityScale = data.defGrav;
        attackSpeed = animator.GetFloat("AttackSpeed");
        allowMove = false;
        //Debug.Log(data.customGravity);
    }

    public override void OnHandle()
    {
        base.OnHandle();
        AttackPressedTimer -= Time.deltaTime;
        MeleeAttack();
    }

    public override void OnFire()
    {
        base.OnFire();
        AttackPressedTimer = 1;
        if (animator.GetFloat("AttackWindow.Open") > 0f)
        {
            if(!allowMove){
                allowMove = true;
                //_behaviors += () => movementController.UseMove();
            }
            if(AttackPressedTimer > 0)
                data.shouldCombo = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public virtual void MeleeAttack()
    {
        //Debug.Log("Animator is " + animator == null + animator.GetBool("Walking"));
        if (animator.GetFloat("Weapon.Active") <= 0f) return;
        //Debug.Log("Animator is " + animator == null);
        for (int i = 0; i < colliderToDamage.Count; i++)
        {
            if(colliderToDamage[i].CompareTag("LivingBody"))
                InflictDamage(colliderToDamage[i]);
        }
    }

    public virtual void InflictDamage(Collider2D targetCollider)
    {
        Controller target = GameHandler.Instance.GetController(targetCollider);
        if(target == null || targetDamaged.Contains(target)) return;
        TeamComponent hitTeamComponent = target.teamComponent;
        if (hitTeamComponent && hitTeamComponent.teamIndex == TeamIndex.Enemy)
        {
            target?.TakeDamage(10);
            Debug.Log("Enemy Has Taken: " + attackIndex + " Damage");
            targetDamaged.Add(target);
        }
    }
}
