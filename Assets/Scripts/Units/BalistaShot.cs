using UnityEngine;

public class BalistaShot : MonoBehaviour, IProjectile
{
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float speed = 50f;

    private Transform target;
    private float damage;

    public void Seek(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    // Update is called once per frame
    void Update()
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
        transform.rotation = Quaternion.LookRotation(dir.normalized);
    }

    void HitTarget()
    {
        // Hit effects.
        GameObject effectInstance = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectInstance, 2f);
        // Damage:
        float generatedDamage = Random.Range(Mathf.RoundToInt(damage / 100 * 75), Mathf.RoundToInt(damage / 100 * 125));
        target.gameObject.GetComponent<Enemy>().TakeDamage(generatedDamage);
        Destroy(gameObject);
    }
}