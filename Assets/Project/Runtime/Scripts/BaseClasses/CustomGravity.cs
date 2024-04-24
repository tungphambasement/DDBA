using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomGravity : MonoBehaviour
{
    public float gravityScale = 1.0f;
    public float gravAccel = 0;
    public static float globalGravity = 40f;
    public float jumpPeakTime = 0;
    public bool useGravity = true;

    Rigidbody2D m_rb;

    void OnEnable()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(!useGravity) return;
        jumpPeakTime = getJumpPeakTime();
        gravityScale += gravAccel;
        Vector2 gravity = -1f * globalGravity * gravityScale * Vector2.up;
        m_rb.AddForce(gravity, ForceMode2D.Force);

    }

    private float getJumpPeakTime(){
        float res = Mathf.Sqrt((2f*m_rb.velocity.y*gravAccel + globalGravity)/(globalGravity*gravAccel*gravAccel)) - 1f/gravAccel;
        return res;
    }
}