using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CollisionListener : MonoBehaviour
{
    public event Action<Collision2D> enterEvents;
    public event Action<Collision2D> stayEvents;
    public event Action<Collision2D> exitEvents;

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision Enter: ");
        enterEvents?.Invoke(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("Collision Enter " + FMATH.CollisionAngle(collision));
        stayEvents?.Invoke(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("Collision Exit");
        exitEvents?.Invoke(collision);
    }
}