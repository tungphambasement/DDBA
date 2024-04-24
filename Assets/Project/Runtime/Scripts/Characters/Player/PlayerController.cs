using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    private float currentHealth;

    public float Health { 
        get{
            return currentHealth;
        }
        private set{
            healthBar?.SetHealth(value);
            currentHealth = value;
        }
    }

    //UI Display
    HealthBar healthBar => data.healthBar;

    private Dictionary<IInteractable,bool> nearby_interactables;
    private IInteractable it_interactables;

    #region ComponentsVariables
    [Header("Component Variables")]
    public PlayerData data;
    public PlayerMovementController movementController;
    public PlayerCombatController combatController;
    #endregion

    public override void AddEffect(Effect effect)
    {
        throw new NotImplementedException();
    }

    public IEnumerator Defeated()
    {
        //animator.SetBool("isDead", true);
        float animationLength = data.animator.GetCurrentAnimatorStateInfo(0).length;
        GetComponent<PlayerInput>().enabled = false;
        yield return new WaitForSeconds(animationLength);
        this.enabled = false;
    }

    public void freezeAnimation()
    {
        data.animator.speed = 0;
    }

    public override void TakeDamage(float Damage)
    {
        Health -= Damage;
        //animator.SetTrigger("TakeDamage");
        data.damageFlash.CallDamageFlash();
        healthBar.SetHealth(Health);
        if (Health <= 0)
        {
            StartCoroutine(Defeated());
            return;
        }
    }

    private void AnimationRetrieveAll(){
        data.anims = new();
        foreach(AnimationClip clip in data.animator.runtimeAnimatorController.animationClips){
            data.anims.Add(clip.name,clip);
        }
    }

    // Awake is always called first
    void Awake()
    {
        nearby_interactables = new();
        movementController.SetController(data);
        combatController.SetController(data);
        currentHealth = data.maxHealth;
        data.defGrav = data.customGravity.gravityScale;
        AnimationRetrieveAll();
    }

    //Start is called before the first frame update
    void Start()
    {
        Debug.Log("Health Bar Exist?: " + healthBar!=null);
        if (healthBar != null)
        {
            healthBar.CustomStart();
            healthBar.SetMaxHealth(data.maxHealth);
            healthBar.SetHealth(Health);
        }
    }

    public void Interact(){
        if(it_interactables != null) 
           it_interactables.OnInteract(this);
    }

    public void AddInteractable(IInteractable interactable){
        nearby_interactables.Add(interactable,true);
    }

    public void RemoveInteractable(IInteractable interactable){
        nearby_interactables.Remove(interactable);
    }
}
