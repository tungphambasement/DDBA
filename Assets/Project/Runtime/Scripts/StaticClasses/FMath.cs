using PlasticGui.WorkspaceWindow;
using UnityEngine;


public static class FMATH{
    public readonly static float fixedDeltaTime = 0.02f;

    public static float CollisionAngle(Collision2D collision){
        Vector2 normal = collision.contacts[0].normal;
        Vector2 vel = collision.relativeVelocity;
        float res = Vector2.Angle(normal,vel);
        if(res > 90f){
            res = 180-res;
        }
        return res;
    }

    public static Vector2 GetAirResistance(CapsuleCollider2D collider, float coef){
        Rigidbody2D rb = collider.attachedRigidbody;
        Vector2 v = rb.velocity;

        return v*fixedDeltaTime*coef;
    }
}