using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] 
    private Tilemap map;

    [SerializeField]
    private List<TileData> tilesDatas;

    [SerializeField] private float minDis, maxDis;


    private Dictionary<TileBase,TileData> dataFromTiles; 

    /*EnemySpawner slimeSpawner;

    private void Awake(){
        dataFromTiles = new Dictionary<TileBase, TileData>();
        slimeSpawner = GetComponent<EnemySpawner>();
        foreach(var TileData in tilesDatas){
            foreach(var tile in TileData.tileBases){
                dataFromTiles.Add(tile,TileData);
            }
        }
    }

    private void Update(){
        if(Input.GetMouseButtonDown(0)){
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //print("Mouse Position: " + mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);
            TileBase clickedTile = map.GetTile(gridPosition);
            //print("At position " + gridPosition + " There is a "+ clickedTile);
        }
    }

    public void SpawnSlime(Vector2 pos){
        Vector2 randPos = new Vector2(Random.Range(pos.x+minDis,pos.x+maxDis),Random.Range(pos.y+minDis,pos.y+maxDis));
        print(randPos);
        EnemySpawner.spawnSlime(randPos);
    }*/
}
