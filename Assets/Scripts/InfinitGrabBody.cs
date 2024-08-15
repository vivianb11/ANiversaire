using UnityEngine;

public class InfinitGrabBody : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject grabedObj;

    public void Interact(Movement player)
    {
        GameObject go = Instantiate(grabedObj);

        player.GrabBody(go.GetComponent<Rigidbody>());
    }
}
