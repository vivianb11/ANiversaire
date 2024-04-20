using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float mouseSensitivity;

    [SerializeField] Transform _grabPos;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] int _pointsCount;
    [SerializeField] float _timeInterval = 0.01f;
    [SerializeField] float _lineMulti;

    Rigidbody rb;
    Camera playerCamera;
    float verticalLookRotation;

    public GameObject InteractionIcon;

    public RectTransform Canvas;
    public GameObject CursorIcon;

    public State _state;

    private Rigidbody _grabbedBody;

    public float throwForce = 500f;
    private float charge;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerCamera = Camera.main;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

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

        if (_grabbedBody != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _grabbedBody.GetComponent<Collider>().enabled = false;
                _lineRenderer.enabled = true;
                charge += Time.deltaTime;
                DrawProjectileTrajectory();
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _grabbedBody.GetComponent<Collider>().enabled = true;
                _lineRenderer.enabled = false;
                _grabbedBody.isKinematic = false;
                _grabbedBody.AddForce(playerCamera.transform.forward * charge * throwForce);

                _grabbedBody.transform.parent = null;

                _grabbedBody = null;
                charge = 0;
            }
        }
        
        if (Input.GetKey(KeyCode.LeftControl))
            _state = State.Interacting;
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

    private void FixedUpdate()
    {
        switch (_state)
        {
            case State.Moving:
                Move();
                break;
            case State.Interacting:
                Move();
                break;
        }
    }

    public void GrabBody(Rigidbody body)
    {
        _grabbedBody = body;
        _grabbedBody.transform.SetParent(_grabPos);
        _grabbedBody.transform.localPosition = Vector3.zero;
        _grabbedBody.transform.localEulerAngles = Vector3.zero;
        _grabbedBody.isKinematic = true;
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
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                InteractionIcon.transform.position = playerCamera.transform.position + ray.direction / 2;
                InteractionIcon.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                    interactable.Interact(this);
            }
            else
                InteractionIcon.SetActive(false);
        }
        else
            InteractionIcon.SetActive(false);
    }

    private void ShowCursor()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;
        CursorIcon.SetActive(true);
    }

    private void HideCursor()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        CursorIcon.SetActive(false);
    }

    private void DrawProjectileTrajectory()
    {
        _lineRenderer.positionCount = Mathf.CeilToInt(_pointsCount / _timeInterval) + 1;
        Vector3 origin = _grabPos.position;
        Vector3 startVel = _lineMulti * (charge * throwForce) * playerCamera.transform.forward / _grabbedBody.mass;
        int i = 0;
        _lineRenderer.SetPosition(i, origin);
        for (float time = 0; time < _pointsCount; time += _timeInterval)
        {
            i++;
            Vector3 point = origin + time * startVel;
            point.y = origin.y + startVel.y * time + (Physics.gravity.y / 2f * time * time);
            _lineRenderer.SetPosition(i, point);
        }
    }
}

public enum State
{
    Moving,
    Interacting,
    Throwing
}