using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }
    private static List<Controller> controllers;
    private static Dictionary<Collider2D, int> ColliderToController;

    void Awake(){
        Instance = this;
    }

    void OnEnable(){
        ColliderToController = new Dictionary<Collider2D, int>();
        BuildDictionary();
    }

    private void BuildDictionary(){
        Controller[] controllersToAdd = FindObjectsOfType<Controller>();
        controllers = new List<Controller>();
        for(int i=0;i<controllersToAdd.Length;i++){
            AddController(controllersToAdd[i]);
        }
    }

    public void AddController(Controller c){
        int index = controllers.Count;
        controllers.Add(c);
        Collider2D[] childColliders = c.GetComponentsInChildren<Collider2D>();
        foreach(Collider2D collider in childColliders){
            ColliderToController.Add(collider, index);
            //Debug.Log("Controller: " + c + " " + collider);
        }
    }

    public Controller GetController(Collider2D collider){
        if(!ColliderToController.ContainsKey(collider)){
            return null;
        }
        return controllers[ColliderToController[collider]];
    }
}