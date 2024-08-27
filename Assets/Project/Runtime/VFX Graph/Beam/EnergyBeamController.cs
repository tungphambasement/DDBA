using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnergyBeamController : MonoBehaviour
{
    [field: SerializeField]
    public VisualEffect beamFX {get; private set;}
    [field: SerializeField]
    public VisualEffect ballFX {get; private set;}

    [SerializeField]
    AnimationCurve DistortionGrowthAC;

    [SerializeField]
    private float maxChargeTime = 1.5f, maxBallSize = 0.8f, majorArcIni = 1.25f, minorArcIni = 0.75f;

    private float chargeTime = 0f, shootTime = 0f, timeStart;
    
    private Action ToUpdate;
    
    private Coroutine shootRoutine; 

    public bool isActive = false;

    public void StartCharge(){
        if(ToUpdate != null || shootRoutine != null ) return; 
        chargeTime = 0f;
        isActive = true;
        beamFX.SendEvent("OnPlay");
        ballFX.SendEvent("OnPlay");
        ballFX.SetBool("IsActive", true);
        ballFX.SetBool("Charging", true);
        beamFX.SetBool("Charging", true);
        ballFX.SetBool("Shooting",false);
        beamFX.SetBool("Shooting",false);
        ballFX.SetBool("Shoot", false);
        beamFX.SetVector3("Arc Transform",ballFX.transform.position-transform.position);
        ToUpdate += ChargeUpdate;
        timeStart = (float)Time.timeAsDouble;
    }

    public void StopCharge(){
        ballFX.SetBool("Charging", false);
        beamFX.SetBool("Charging", false);
        ballFX.SetBool("IsActive", false);
        ToUpdate -= ChargeUpdate;
        isActive = false;
    }

    public void Shoot(){
        ballFX.SetBool("IsActive", true);
        shootRoutine = StartCoroutine(ShootCoroutine());
        isActive = true;
    }

    private void ChargeUpdate(){
        chargeTime += Time.deltaTime;
        /*if(chargeTime >= maxChargeTime){
            StopCharge();
        }*/
    }

    void Update(){
        ToUpdate?.Invoke();
    }

    private IEnumerator ShootCoroutine(){
        Debug.Log(chargeTime);
        //yield return new WaitForSeconds(Mathf.Max(0.7f-(chargeTime-1.5f),0f));
        ballFX.SetFloat("ShootTime", (float) Time.timeAsDouble - timeStart);
        ballFX.SetBool("Shoot",true);
        yield return new WaitForSeconds(0.075f);
        ballFX.SetBool("Shooting", true);
        beamFX.SetBool("Shooting", true);
        ballFX.SetFloat("Ball Flutter",10f);
        yield return new WaitForSeconds(0.075f);
        ballFX.SetBool("Shoot",false);
        yield return new WaitForSeconds(2f);
        beamFX.SendEvent("OnStop");
        ballFX.SendEvent("OnStop");
        isActive = false;
        ballFX.SetBool("IsActive", false);
        shootRoutine = null;
    }
}
