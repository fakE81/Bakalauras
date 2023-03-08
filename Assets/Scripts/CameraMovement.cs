using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Movement speed;
    public float panSpeed = 30f;
    public float scrollSpeed = 5f;


    // Max zoom in/out
    public float minFOV = 10f;

    public float maxFOV = 120f;

    private Camera cam;

    // Update is called once per frame
    private void Start()
    {
        cam = this.GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        var fieldOfView = cam.fieldOfView - scroll * scrollSpeed;
        fieldOfView = Mathf.Clamp(fieldOfView, minFOV, maxFOV);
        cam.fieldOfView = fieldOfView;
    }
}