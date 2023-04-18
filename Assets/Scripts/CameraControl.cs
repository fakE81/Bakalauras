using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float rotationSpeed = 2f;
    public float scrollSpeed = 5f;
    public float minFOV = 10f;
    public float maxFOV = 120f;
    private Camera cam;
    private void Start()
    {
        cam = this.GetComponent<Camera>();
    }
    
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
            transform.Rotate(Vector3.up, -mouseX, Space.World);
            transform.Rotate(Vector3.right, mouseY, Space.Self);
        }
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        var fieldOfView = cam.fieldOfView - scroll * scrollSpeed;
        fieldOfView = Mathf.Clamp(fieldOfView, minFOV, maxFOV);
        cam.fieldOfView = fieldOfView;
    }
}
