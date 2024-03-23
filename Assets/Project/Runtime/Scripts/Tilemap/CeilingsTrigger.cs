using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CeilingsTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;

    [SerializeField] UnityEvent onTriggerExit;


    void OnTriggerEnter(Collider Other){
        onTriggerEnter.Invoke();
    }

    void OnTriggerExit(Collider Other){
        onTriggerExit.Invoke();
    }
}
