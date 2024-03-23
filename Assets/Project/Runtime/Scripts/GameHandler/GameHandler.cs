using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public PlayerController[] playerControllers;
    public EnemyController[] enemyControllers;

    void Awake(){
        playerControllers = FindObjectsOfType<PlayerController>();
        enemyControllers = FindObjectsOfType<EnemyController>();
    }


}