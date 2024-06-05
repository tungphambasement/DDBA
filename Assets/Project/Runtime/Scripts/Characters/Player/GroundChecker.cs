using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundChecker : MonoBehaviour {
    HashSet<int> collidersInContact = new();
    Collider2D thisCollider;
    void OnEnable(){
       thisCollider =  GetComponent<Collider2D>();
    }

    public bool isGrounded()
    {
        //Debug.Log("Is Grounded: " + (collidersInContact.Count > 0));
        return collidersInContact.Count > 0;
    }

    private bool IsAnotherObject(Collider2D other){
        return GameHandler.Instance.GetController(other) != GameHandler.Instance.GetController(thisCollider);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(collidersInContact.Contains(other.GetInstanceID())) return;
        if(IsAnotherObject(other)){
            collidersInContact.Add(other.GetInstanceID());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(collidersInContact.Contains(other.GetInstanceID())){
            collidersInContact.Remove(other.GetInstanceID());
        }
    }
}