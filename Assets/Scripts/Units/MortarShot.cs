using System;
using UnityEngine;

public class MortarShot : MonoBehaviour, IProjectile
{
    public float speed = 10f;
    public float gravity = -9.81f;
    public float max;
    public Vector3 pos;
    
    
    private Vector3 targetPosition;
    private float maxHeight;

    // What is target :) 
    public void Seek(Transform target, float damage)
    {
        throw new NotImplementedException();
    }
    
    private void Start()
    {
        Shoot(pos, max);
    }

    public void Shoot(Vector3 targetPosition, float maxHeight)
    {
        this.targetPosition = targetPosition;
        this.maxHeight = maxHeight;
        GetComponent<Rigidbody>().velocity = CalculateLaunchVelocity();
    }

    private Vector3 CalculateLaunchVelocity()
    {
        float displacementY = targetPosition.y - transform.position.y;
        Vector3 displacementXZ = new Vector3(targetPosition.x - transform.position.x, 0f, targetPosition.z - transform.position.z);
        float time = Mathf.Sqrt(-2f * maxHeight / gravity) + Mathf.Sqrt(2f * (displacementY - maxHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2f * gravity * maxHeight);
        Vector3 velocityXZ = displacementXZ / time;
        return velocityXZ + velocityY * -Mathf.Sign(gravity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}