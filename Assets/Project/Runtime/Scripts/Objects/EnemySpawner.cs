using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float guardingRange = 0.2f;

    private float elapsed;
    public bool canSpawn;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!canSpawn) return;
        elapsed += Time.deltaTime;
        if(elapsed > spawnRate){
            elapsed = 0;
            Transform positionToSpawn = this.transform;
            Vector2 originPos = positionToSpawn.position;    
            foreach(GameObject monsters in enemyPrefabs){
                Transform new_positionToSpawn = new GameObject().transform;
                new_positionToSpawn.position = new Vector2(
                    originPos.x+Random.Range(-guardingRange,guardingRange),
                    originPos.y+Random.Range(-guardingRange,guardingRange)
                    );
                Instantiate(monsters,new_positionToSpawn);
            }
        }
    }
}   
