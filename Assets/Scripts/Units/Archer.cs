using UnityEngine;

public class Archer : MonoBehaviour
{
    private Transform target;
    [Header("Attributes")] public float range = 5f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public float damage = 10f;

    [Header("Unity Setup Fields")] public string enemyTag = "Enemy";
    public float turnSpeed = 3f;


    public GameObject arrowPrefab;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (shortestDistance > distanceToEnemy)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        // Seek enemies, Target lock on;
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);


        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        // Every second by 1
        fireCountdown -= Time.deltaTime;
    }


    void Shoot()
    {
        GameObject arrowGO = (GameObject)Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        Arrow arrow = arrowGO.GetComponent<Arrow>();
        arrow.setDamage(damage);
        if (arrow != null)
        {
            arrow.Seek(target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void OnMouseDown()
    {
        GameManager.instance.handleInformationUI(gameObject.transform, damage, range, fireRate, turnSpeed);
    }
}