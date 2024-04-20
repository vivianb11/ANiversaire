using UnityEngine;
using UnityEngine.Events;

public class PinInteraction : MonoBehaviour, IInteractable
{
    [HideInInspector] public UnityEvent<int> OnInteract;

    public int number;

    public void Interact(Movement player)
    {
        OnInteract.Invoke(number);
    }
}
