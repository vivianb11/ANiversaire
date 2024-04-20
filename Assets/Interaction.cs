using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteract;

    public void Interact(Movement player)
    {
        OnInteract?.Invoke();
    }
}
