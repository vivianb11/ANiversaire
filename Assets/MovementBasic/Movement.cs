using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float mouseSensitivity;

    Rigidbody rb;
    Camera playerCamera;
    float verticalLookRotation;

    public GameObject InteractionIcon;

    public RectTransform Canvas;
    public GameObject CursorIcon;
    private Vector3 rayDir;

    public State _state;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _state = State.Moving;

        InteractionIcon.SetActive(false);
        CursorIcon.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
            HideCursor();
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            ShowCursor();

        switch (_state)
        {
            case State.Moving:
                Rotate();
                InteractLogic();
                break;
            case State.Interacting:
                InteractLogic(); 
                break;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            _state = State.Interacting;
        else if (Input.GetKey(KeyCode.LeftShift))
            _state = State.Throwing;
        else
            _state = State.Moving;

        switch (_state)
        {
            case State.Moving:
                Move();
                break;
            case State.Interacting:
                Move();
                break;
            case State.Throwing:
                Debug.Log("Throwing not yet implemented");
                break;
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * movementSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + transform.TransformDirection(movement));
    }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalLookRotation += mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        playerCamera.transform.localEulerAngles = new Vector3(-verticalLookRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    void InteractLogic()
    {
        if (_state == State.Interacting)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.x -= (Canvas.sizeDelta / 2f).x;
            mousePos.y -= (Canvas.sizeDelta / 2f).y;

            CursorIcon.transform.localPosition = mousePos;
        }

        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 3))
        {
            List<IInteractable> interactable = new List<IInteractable>();

            MonoBehaviour[] scripts = hit.collider.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour script in scripts)
            {
                if (script is IInteractable)
                    interactable.Add(script as IInteractable);
            }

            if (interactable.Count == 0)
            {
                InteractionIcon.SetActive(false);

                return;
            }

            InteractionIcon.SetActive(true);
            InteractionIcon.transform.position = hit.collider.transform.position;

            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach (IInteractable interact in interactable)
                {
                    interact.Interact();
                }
            }
        }
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        CursorIcon.SetActive(true);
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CursorIcon.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (EditorApplication.isPlaying)
            Gizmos.DrawRay(playerCamera.transform.position, rayDir * 3);
    }
}

public enum State
{
    Moving,
    Interacting,
    Throwing
}