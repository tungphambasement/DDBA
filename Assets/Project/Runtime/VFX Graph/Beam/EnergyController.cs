

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class EnergyController : MonoBehaviour
{
    [field: SerializeField]
    public VisualEffect beamFX {get; private set;}

    [SerializeField]
    private float maxChargeTime = 1.5f, maxBallSize = 0.8f, majorArcIni = 1.25f, minorArcIni = 0.75f;

    private Action ToUpdate;
    
    private Coroutine shootRoutine; 

    public bool isActive = false;

    public void StartCharge(){
        if(ToUpdate != null || shootRoutine != null ) return; 
        isActive = true;
        beamFX.SendEvent("OnPlay");
        beamFX.SetBool("Charging", true);
        beamFX.SetBool("Shooting",false);
    }

    public void StopCharge(){
        beamFX.SetBool("Charging", false);
        isActive = false;
    }

    public void Shoot(){
        shootRoutine = StartCoroutine(ShootCoroutine());
        isActive = true;
    }

    void Update(){
        ToUpdate?.Invoke();
    }

    private IEnumerator ShootCoroutine(){
        yield return new WaitForSeconds(0.075f);
        beamFX.SetBool("Shooting", true);
        yield return new WaitForSeconds(0.075f);
        yield return new WaitForSeconds(2f);
        beamFX.SendEvent("OnStop");
        isActive = false;
        shootRoutine = null;
    }
}