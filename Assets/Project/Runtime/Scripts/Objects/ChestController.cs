using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestController : MonoBehaviour, IInteractable
{
    private Animator animator;
    EnemySpawner enemySpawner;
    private bool isOpened;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        enemySpawner = GetComponent<EnemySpawner>();
    }

    void Start()
    {
        isOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOpen(){
        Debug.Log("chest Opened");
        animator.SetBool("Open",true);
        enemySpawner.canSpawn = false;
    }

    public void OnInteract(Controller eventFiredController)
    {
        if(!isOpened) OnOpen();
    }


}
