using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    void Update()
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
                Rotate();
                InteractLogic();

                break;
            case State.Interacting:
                Move();
                InteractLogic();
                break;
            case State.Throwing:
                Debug.Log("Throwing not yet implemented");
                break;
        }


        InteractLogic();

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

            rayDir = playerCamera.transform.forward + playerCamera.transform.right * Input.GetAxis("Mouse X") + playerCamera.transform.up * Input.GetAxis("Mouse Y");

            CursorIcon.transform.position = playerCamera.transform.position + rayDir * 3;
        }
        else
        {
            if (CursorIcon.activeSelf)
                CursorIcon.SetActive(false);

            rayDir = playerCamera.transform.forward;
        }

        RaycastHit hit;
        
        if (Physics.Raycast(playerCamera.transform.position, rayDir, out hit, 3))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                InteractionIcon.SetActive(true);
                InteractionIcon.transform.position = hit.collider.transform.position;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
        }
        else
        {
            InteractionIcon.SetActive(false);
        }
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