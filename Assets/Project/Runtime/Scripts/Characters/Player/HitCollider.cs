using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HitCollider : MonoBehaviour
{
    public List<Collider2D> CollidersEntered; 

    void OnEnable(){
        CollidersEntered = new();
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(!CollidersEntered.Contains(other)) CollidersEntered.Add(other);
    }

    public void OnTriggerExit2D(Collider2D other){
    }
}
