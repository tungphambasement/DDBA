using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] BoxCollider2D groundChecker;
    private int grounded;

    public bool isGrounded()
    {
        return grounded > 0;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Obstacle Entered");
        if(other.name != "Player_Body") grounded++;
    }

    public void OnTriggerExit2D(Collider2D other)
    {

        //Debug.Log("Obstacle Exited");
        if(other.name != "Player_Body") grounded--;
    }
}