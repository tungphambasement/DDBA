using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class DashTrail : MonoBehaviour{
    private VisualEffect dashTrailEffect; 
    
    public ObjectPool<DashTrail> dashTrailPool;

    void OnEnable(){
        dashTrailEffect = GetComponent<VisualEffect>();
    }
    List<Ray> raysToDraw = new();

    public void SpawnEffect(Vector3 velocity){
        //Debug.Log(this.transform.position + " " + "Playing");
        raysToDraw.Add(new Ray(transform.position,velocity));
        dashTrailEffect.SetVector2("Velocity", velocity);
        dashTrailEffect.Play();
        StartCoroutine(Pool());
    }   

    private IEnumerator Pool(){
        yield return new WaitForSeconds(3);
        //Debug.Log("Pooled");
        dashTrailPool.ReturnObject(this);
    }

    public void OnDrawGizmosSelected(){
        if(dashTrailEffect != null){
            foreach(var ray in raysToDraw){ Gizmos.DrawRay(ray);}
        }
    }
}