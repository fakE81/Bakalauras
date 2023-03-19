using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform target;
    public GameObject impactEffect;
    public float speed = 50f;
    public float damage = 10f;

    public void Seek(Transform _target)
    {
        target = _target;
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
        GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectInstance, 2f);
        // Calculate damage:
        float generatedDamage = Random.Range(Mathf.RoundToInt(damage/100*75), Mathf.RoundToInt(damage/100*125));
        target.gameObject.GetComponent<Enemy>().TakeDamage(generatedDamage);
        // Check if target is dead:
        if (target.gameObject.GetComponent<Enemy>().isDead)
        {
            PlayerStatisticsManager.instance.AddExeperienceToBalista(target.gameObject.GetComponent<Enemy>().blueprint.givesExperience);
        }
        Destroy(gameObject);
    }

    public void setDamage(float damage)
    {
        this.damage = damage;
    }
}