


using System.Collections.Generic;
using UnityEngine;

public class ChestTrigger : ZoneTrigger
{
    HashSet<int> enteredPlayers = new();
    Transform temp_ColliderParent;

    public override bool CanTrigger(Collider2D other)
    {
        temp_ColliderParent = other.transform.parent;
        //Debug.Log((temp_ColliderParent.tag == "Player") + " " + (!enteredPlayers.Contains(temp_ColliderParent.GetInstanceID())));
        return temp_ColliderParent.tag == "Player" && !enteredPlayers.Contains(temp_ColliderParent.GetInstanceID());
    }

    public override void FireEnterEvent(Collider2D other)
    {
        Debug.Log("Entered");
        enteredPlayers.Add(temp_ColliderParent.GetInstanceID());
        PlayerController playerController = temp_ColliderParent.GetComponent<PlayerController>();
        playerController.AddInteractable(GetComponentInParent<IInteractable>());
    }

    public override void FireExitEvent(Collider2D other)
    {
        enteredPlayers.Remove(temp_ColliderParent.GetInstanceID());
        PlayerController playerController = temp_ColliderParent.GetComponent<PlayerController>();
        playerController.RemoveInteractable(GetComponentInParent<IInteractable>());
    }
}