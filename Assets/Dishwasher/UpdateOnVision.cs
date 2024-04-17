using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

public class UpdateOnVision : MonoBehaviour , IInteractable
{
    [SerializeField] GameObject[] allPlatesStates;
    [SerializeField] Transform platesSpawnPoint;
    GameObject instantiatedPlates;
    Camera playerCam;

    [SerializeField] bool loopStates = false;

    public UnityEvent onOutOfViw;

    int currentState;
    bool wasOutsideView = false;

    void Start()
    {
        instantiatedPlates = Instantiate(allPlatesStates[0], platesSpawnPoint);
        currentState = 0;

        playerCam = Camera.main;
    }

    void Update()
    {
        Bounds bounds = GetComponent<Renderer>().bounds;

        bool isOutsideView = true;
        foreach (Vector3 vertex in GetBoundsVertices(bounds))
        {
            Vector3 viewportPoint = playerCam.WorldToViewportPoint(vertex);
            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z >= 0)
            {
                isOutsideView = false;
                break;
            }
        }

        if (isOutsideView && !wasOutsideView)
        {
            UpdatePlatesState();
        }

        wasOutsideView = isOutsideView;
    }

    Vector3[] GetBoundsVertices(Bounds bounds)
    {
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        Vector3[] vertices = new Vector3[8];
        vertices[0] = center + new Vector3(-extents.x, -extents.y, -extents.z);
        vertices[1] = center + new Vector3(extents.x, -extents.y, -extents.z);
        vertices[2] = center + new Vector3(-extents.x, extents.y, -extents.z);
        vertices[3] = center + new Vector3(extents.x, extents.y, -extents.z);
        vertices[4] = center + new Vector3(-extents.x, -extents.y, extents.z);
        vertices[5] = center + new Vector3(extents.x, -extents.y, extents.z);
        vertices[6] = center + new Vector3(-extents.x, extents.y, extents.z);
        vertices[7] = center + new Vector3(extents.x, extents.y, extents.z);

        return vertices;
    }

    [Button("Update Plates State")]
    void UpdatePlatesState()
    {
        Debug.Log("Object has completely left the vision of the player camera.");

        Destroy(instantiatedPlates);
        currentState = Mathf.Clamp(currentState + 1, 0, allPlatesStates.Length - 1);

        if (loopStates && currentState == allPlatesStates.Length - 1)
            currentState = 0;

        instantiatedPlates = Instantiate(allPlatesStates[currentState], platesSpawnPoint);
    }

    [Button("Reset States")]
    private void ResetStates()
    {
        currentState = 0;
    }

    public void Interact()
    {
        Destroy(instantiatedPlates);

        if (loopStates && currentState == 0)
            currentState = allPlatesStates.Length - 1;
        else
            currentState = Mathf.Clamp(currentState - 1, 0, allPlatesStates.Length - 1);

        instantiatedPlates = Instantiate(allPlatesStates[currentState], platesSpawnPoint);
    }
}
