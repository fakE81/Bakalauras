using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Movement speed;
    public float movementSpeed = 30f;
    

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical) * movementSpeed * Time.deltaTime;
        transform.position += transform.TransformDirection(new Vector3(moveDirection.x, 0f, moveDirection.z));
        transform.position = new Vector3(transform.position.x, 13f, transform.position.z);
    }
}