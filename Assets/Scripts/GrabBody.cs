using UnityEngine;

public class GrabBody : MonoBehaviour, IInteractable
{
    public void Interact(Movement player)
    {
        player.GrabBody(GetComponent<Rigidbody>());
    }
}
