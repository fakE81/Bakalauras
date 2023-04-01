using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MortarShot : MonoBehaviour, IProjectile
{
    public float radius = 10f;
    public float gravity = -9.81f;
    public float max;
    public LayerMask layerMask;
    private float damage;
    [SerializeField] private GameObject impactEffect;


    private Vector3 targetPosition;
    private float maxHeight;
    
    public void Seek(Transform target, float damage)
    {
        this.damage = damage;
        Shoot(target.position, max);
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
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius, layerMask);
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float generatedDamage = Random.Range(Mathf.RoundToInt(damage / 100 * 75), Mathf.RoundToInt(damage / 100 * 125));
                collider.GetComponent<Enemy>().TakeDamage(generatedDamage);
            }
        }
        GameObject effectInstance = Instantiate(impactEffect, transform.position, impactEffect.transform.rotation);
        Destroy(effectInstance, 2f);
        Destroy(gameObject);
    }
}