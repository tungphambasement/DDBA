using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerGroundChecker : MonoBehaviour
{
    CapsuleCollider2D playerCollider;
    Rigidbody2D rb;
    HashSet<int> collidersInContact = new();
    Vector2 playerPos => playerCollider.offset + rb.position;
    Vector2 colliderSize => playerCollider.size;

    void OnEnable(){
        playerCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public bool isGrounded()
    {
        Debug.Log("Is Grounded: " + (collidersInContact.Count > 0));
        return collidersInContact.Count > 0;
    }

    private bool IsAnotherObject(Collider2D other){
        return GameHandler.Instance.GetController(other) != GameHandler.Instance.GetController(playerCollider);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collidersInContact.Contains(collision.collider.GetInstanceID())) return;
        bool isBelow = false;
        foreach(ContactPoint2D contacts in collision.contacts){
            if(contacts.point.y <= playerPos.y-colliderSize.y/2+colliderSize.x/2+0.2f) {
                isBelow = true; 
                break;
            }
        }
        if(isBelow && IsAnotherObject(collision.collider)){
            collidersInContact.Add(collision.collider.GetInstanceID());
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collidersInContact.Contains(collision.collider.GetInstanceID())) return;
        bool isBelow = false;
        foreach(ContactPoint2D contacts in collision.contacts){
            if(contacts.point.y <= playerPos.y-colliderSize.y/2+colliderSize.x/2+0.2f) {
                isBelow = true; 
                break;
            }
        }
        if(isBelow && IsAnotherObject(collision.collider)){
            collidersInContact.Add(collision.collider.GetInstanceID());
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if(collidersInContact.Contains(collision.collider.GetInstanceID())){
            collidersInContact.Remove(collision.collider.GetInstanceID());
        }
    }
}