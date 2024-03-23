
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Controller : MonoBehaviour
{
    public TeamComponent teamComponent;

    public abstract void TakeDamage(float Damage);

    public abstract void AddEffect(Effect effect);
}

public interface IMovable{
    public void moveSelf(Vector2 direction);    
}

public interface ICanInteract{
    public void AddInteractables(IInteractable interactable);

    public void DeleteInteractables(IInteractable interactable);
}