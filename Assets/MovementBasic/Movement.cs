using System;
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

    public GameObject CursorIcon;
    private Vector3 rayDir;
    private Vector3 mousePos;

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
        if (Input.GetKey(KeyCode.LeftControl))
            _state = State.Interacting;
        else if (Input.GetKey(KeyCode.LeftShift))
            _state = State.Throwing;
        else
            _state = State.Moving;

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

    void FixedUpdate()
    {
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
            if (!CursorIcon.activeSelf)
                CursorIcon.SetActive(true);

            mousePos.x += Input.GetAxis("Mouse X") * mouseSensitivity;
            mousePos.y += Input.GetAxis("Mouse Y") * mouseSensitivity;

            rayDir = playerCamera.transform.forward * 3 + playerCamera.transform.right * mousePos.x + playerCamera.transform.up * mousePos.y;

            CursorIcon.transform.position = playerCamera.transform.position + rayDir;
        }
        else
        {
            if (CursorIcon.activeSelf)
                CursorIcon.SetActive(false);

            if (mousePos != Vector3.zero)
                mousePos = Vector3.zero;

            rayDir = playerCamera.transform.forward;
        }

        RaycastHit hit;
        
        if (Physics.Raycast(playerCamera.transform.position, rayDir, out hit, 3))
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

            InteractionIcon.transform.position = playerCamera.transform.position + rayDir/2;

            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach (IInteractable interact in interactable)
                {
                    interact.Interact();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (EditorApplication.isPlaying)
            Gizmos.DrawRay(playerCamera.transform.position, rayDir * 3);
    }
}

// Interactable interface
public interface IInteractable
{
    void Interact();
}

public enum State
{
    Moving,
    Interacting,
    Throwing
}