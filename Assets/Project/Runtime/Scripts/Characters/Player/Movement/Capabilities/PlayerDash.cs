using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : PlayerBaseMovement
{
    private AfterImageController afterImagePrefab => data.afterImagePrefabs.GetComponent<AfterImageController>();
    private ObjectPool<AfterImageController> afterImagePooler;

    #region Pooling
    private void TurnOnAfterImage(AfterImageController o){
        o.gameObject.SetActive(true);
        o.SetSprite(data.spriteRenderer.sprite);
        o.transform.position = _GFX.transform.position;
        o.transform.rotation = _GFX.transform.rotation;
        o.transform.localScale = _GFX.transform.localScale;
        lastImagePos = o.transform.position;
        o.objectPool = afterImagePooler;
    }
    
    private  void TurnOffAfterImage(AfterImageController o){
        o.gameObject.SetActive(false);
        o.transform.SetParent(data.AfterImages);
    }

    private AfterImageController FactoryAfterImage(){
        return MonoBehaviour.Instantiate(afterImagePrefab);
    }
    #endregion

    public PlayerDash(PlayerData _data) : base(_data)
    {
        
    }

    public override void SelfInitialize()
    {
        base.SelfInitialize();
        afterImagePooler = new ObjectPool<AfterImageController>(FactoryAfterImage,TurnOnAfterImage,TurnOffAfterImage,10);
    }

    public override void SetUpConstants()
    {
        base.SetUpConstants();
    }   

    private Coroutine dashRoutine;

    public void DashCharacter(){
        data.canMove = false;
        if(dashRoutine == null)
            dashRoutine = data.StartCoroutine(StartDash());
    }

    public void OnHitWall(){
        appliedVelocity = Vector2.zero;
        StopDashCharacter();
    }

    public void StopDashCharacter(){
        if(dashRoutine == null) return;
        data.StopCoroutine(dashRoutine);
        data.canMove = true;
        data.isDashing = false;
        customGravity.useGravity = true;
        dashRoutine = null;
        animationManager.RemoveAnim(4);
        rb.velocity -= appliedVelocity;
        appliedVelocity = Vector2.zero;
        Debug.Log("Stopped Dash Routine");
    }
    private float lastInput; 
    private float dashTime;
    private Vector3 lastImagePos;
    private Vector2 appliedVelocity; 

    private IEnumerator StartDash(){
        animationManager.ForceAdd(4, "AirFloatLoop");
        data.isDashing = true;
        data.canMove = false;
        customGravity.useGravity = false;
        lastInput = _GFX.transform.localScale.x;
        appliedVelocity = new Vector2(lastInput * data.movementSpeed * 2 - rb.velocity.x, 0) * data.velocityMult;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.velocity += appliedVelocity;
        dashTime = Time.time;
        lastImagePos = rb.transform.position;
        while(Time.time - dashTime < data.dashTime){
            afterImagePooler.GetObject();
            yield return new WaitUntil(() => ((rb.transform.position-lastImagePos).magnitude > data.afterImageDistance) || Time.time - dashTime >= data.dashTime);
        }
        StopDashCharacter();
    }
}
