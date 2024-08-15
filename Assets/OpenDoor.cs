//using UnityEngine;
//using DG.Tweening;

//public class OpenDoor : MonoBehaviour, IInteractable
//{
//    [SerializeField] Transform pivot;
//    [SerializeField] float angle = -120;
//    float originalY;

//    [Range(0.5f,5f)]
//    [SerializeField] float speed = 0.5f;

//    bool opened = false;

//    public void Start()
//    {
//        originalY = pivot.rotation.y;
//    }

//    public void Interact(Movement player)
//    {
//        if (opened) return;

//        opened = true;

//        pivot.DORotate(new(0, angle + originalY, 0),speed);
//    }
//}
using UnityEngine;
using DG.Tweening;

public class OpenDoor : MonoBehaviour, IInteractable
{
    [SerializeField] Transform pivot;
    [SerializeField] float angle = -120;
    float originalY;

    [Range(0.5f, 5f)]
    [SerializeField] float speed = 0.5f;

    bool opened = false;

    public void Start()
    {
        originalY = pivot.eulerAngles.y; // Utilisation de eulerAngles pour obtenir l'angle en degrés
    }

    public void Interact(Movement player)
    {
        if (opened) return;

        opened = true;

        // Calculer la nouvelle rotation en prenant en compte l'angle d'ouverture par rapport à l'orientation actuelle
        pivot.DORotate(new Vector3(0, angle + originalY, 0), speed);
    }
}