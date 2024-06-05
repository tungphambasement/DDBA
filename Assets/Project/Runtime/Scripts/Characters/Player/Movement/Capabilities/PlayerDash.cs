using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerDash : PlayerBaseMovement
{
    private AfterImageController afterImagePrefab => data.afterImagePrefabs.GetComponent<AfterImageController>();
    private ObjectPool<AfterImageController> afterImagePooler;
    private ObjectPool<DashTrail> dashTrailPooler;

    private float alphaSet = 0.8f;
    private float alphaMult = 0.9f;
    #region Pooling
    private void TurnOnDashTrail(DashTrail o){
        o.transform.position = rb.position+data.playerHitbox.offset;
        o.gameObject.SetActive(true);
        o.dashTrailPool = dashTrailPooler;
        o.SpawnEffect(rb.velocity.normalized*1.5f/alphaMult);
    }

    private void TurnOffDashTrail(DashTrail o){
        o.gameObject.SetActive(false);
    }

    private DashTrail FactoryDashTrail(){
        return MonoBehaviour.Instantiate(data.dashTrail,data.AfterImages);
    }

    private void TurnOnAfterImage(AfterImageController o){
        o.SetSprite(data.spriteRenderer.sprite);
        o.transform.position = _GFX.transform.position;
        o.transform.rotation = _GFX.transform.rotation;
        o.transform.localScale = _GFX.transform.localScale;
        lastImagePos = o.transform.position;
        o.alphaSet = alphaSet;
        o.alphaMult = alphaMult;
        o.objectPool = afterImagePooler;
        o.gameObject.SetActive(true);
    }
    
    private  void TurnOffAfterImage(AfterImageController o){
        o.transform.SetParent(data.AfterImages);
        o.gameObject.SetActive(false);
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
        dashTrailPooler = new ObjectPool<DashTrail>(FactoryDashTrail,TurnOnDashTrail,TurnOffDashTrail,10);
    }

    public override void SetUpConstants()
    {
        base.SetUpConstants();
    }   

    private Coroutine dashRoutine, stopRoutine;

    public void DashCharacter(){
        data.canMove = false;
        if(dashRoutine == null && stopRoutine == null)
            StartDash();
    }

    public void OnHitWall(){
        appliedForce = Vector2.zero;
        StopDashCharacter();
    }

    public void StopDashCharacter(){
        data.StopCoroutine(stopRoutine);
        if(dashRoutine != null) data.StopCoroutine(dashRoutine);
        rb.drag = 0.3f;
        data.canMove = true;
        data.isDashing = false;
        customGravity.useGravity = true;
        dashRoutine = null;
        stopRoutine = null;
        animationManager.RemoveAnim(4);
    }
    private float lastInput; 
    private float dashTime;
    private Vector3 lastImagePos;
    private Vector2 appliedForce; 

    private void StartDash(){
        alphaSet = 0.85f;
        alphaMult = 0.93f;
        if(animationManager.currentAnim.StartsWith("Idle")) animationManager.ForceAdd(4, "AirFloatLoop");
        data.isDashing = true;
        data.canMove = false;
        customGravity.useGravity = false;
        lastInput = _GFX.transform.localScale.x;
        appliedForce = new Vector2(lastInput * data.movementSpeed * 2f - rb.velocity.x, 0);
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.drag = 5f;
        rb.AddForce(appliedForce, ForceMode2D.Impulse);
        dashTime = Time.time;
        lastImagePos = rb.transform.position;

        stopRoutine = data.StartCoroutine(StopDashCountDown());
        dashRoutine = data.StartCoroutine(AfterImageRoutine());
    }

    private IEnumerator StopDashCountDown(){
        yield return new WaitForSeconds(data.dashTime);
        //Debug.Log("Stopping Dash");
        StopDashCharacter();
    }

    private IEnumerator AfterImageRoutine(){
        yield return new WaitForFixedUpdate();
        while(Mathf.Abs(rb.velocity.x) > 0){
            afterImagePooler.GetObject();
            dashTrailPooler.GetObject();
            alphaSet = Mathf.Min(alphaSet*1.1f,1f);
            alphaMult = Mathf.Min(alphaMult*1.01f,0.98f);
            yield return new WaitUntil(() => (rb.transform.position-lastImagePos).magnitude > data.afterImageDistance);
        }
        //Debug.Log("After Image Stopped");
        dashRoutine = null;
    }
}
