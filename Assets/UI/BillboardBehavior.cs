using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardBehavior : MonoBehaviour
{
    Camera playerCamera;

    private void Awake()
    {
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object to face the camera
        transform.LookAt(transform.position + playerCamera.transform.rotation * Vector3.forward,
                       playerCamera.transform.rotation * Vector3.up);
    }
}
