using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomGravity : MonoBehaviour
{
    public float gravityScale = 1.0f;

    public static float globalGravity = 40f;

    public bool useGrav = true;

    Rigidbody2D m_rb;

    void OnEnable()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (useGrav)
        {
            Vector2 gravity = -1f * globalGravity * gravityScale * Vector2.up;
            m_rb.AddForce(gravity, ForceMode2D.Force);
        }
    }
}