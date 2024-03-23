using UnityEngine;
using UnityEngine.Events;


public abstract class ZoneTrigger : MonoBehaviour
{
    [SerializeField] protected UnityEvent onTriggerEnter, onTriggerExit;
    public abstract bool CanTrigger(Collider2D other);

    protected void OnTriggerEnter2D(Collider2D other){
        if(!CanTrigger(other)) return;
        FireEnterEvent(other);
    }

    public abstract void FireEnterEvent(Collider2D other);

    protected void OnTriggerExit2D(Collider2D other){
        if(!CanTrigger(other)) return;
        FireExitEvent(other);
    }

    public abstract void FireExitEvent(Collider2D other);
}