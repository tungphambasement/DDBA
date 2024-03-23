using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public List<Collider> CollidersEntered; 

    void OnEnable(){
        CollidersEntered = new();
    }

    public void OnTriggerEnter(Collider other){
        if(!CollidersEntered.Contains(other)) CollidersEntered.Add(other);
    }

    public void OnTriggerExit(Collider other){
    }
}
