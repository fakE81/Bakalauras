using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShot : MonoBehaviour, IProjectile
{
    private float slow;
    private Transform target;
    public LayerMask layerMask;
    [SerializeField] private int speed;
    [SerializeField] private int radius;
    [SerializeField] private GameObject impactEffect;
    
    public void Seek(Transform target, float slowness)
    {
        slow = slowness;
        this.target = target;
    }

    public void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // distance length 
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius, layerMask);
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<Enemy>().TakeSlowness(slow/100);
            }
        }
        GameObject effectInstance = Instantiate(impactEffect, transform.position, impactEffect.transform.rotation);
        Destroy(effectInstance, 2f);
        Destroy(gameObject);
    }
}
