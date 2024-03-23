using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MeleeBaseState : State
{
    #region Local Var
    public float duration;
    protected List<Collider> colliderToDamage;
    private List<Controller> targetDamaged;
    protected List<State> combatStates;
    protected bool shouldCombo;
    protected float attackIndex;
    // Input buffer Timer
    private float AttackPressedTimer;
    protected string animation_name;
    protected AnimationClip clip;
    #endregion

    #region Component Variables
    protected PlayerData data;
    protected PlayerController playerController;
    protected Animator animator;
    protected HitCollider hitCollider;
    protected Rigidbody2D rb;
    protected AnimationManager animationManager;
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

    protected void UpdateAnim(){
        clip = data.anims[animation_name];
    }

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        AttackPressedTimer = 0;
        shouldCombo = false;
        colliderToDamage = hitCollider.CollidersEntered = new();
        targetDamaged = new();
        data.customGravity.gravityScale = data.defGrav;
        UpdateAnim();
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
        if (animator.GetFloat("AttackWindow.Open") > 0f && AttackPressedTimer > 0)
        {
            shouldCombo = true;
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

    public virtual void InflictDamage(Collider targetCollider)
    {
        Controller target = targetCollider.GetComponentInParent<Controller>();
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
