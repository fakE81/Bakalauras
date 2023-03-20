using UnityEngine;

public class Balista : MonoBehaviour
{
    private Transform target;
    private float fireCountdown = 0f;
    [SerializeField]private TowerInformation towerInformation;
    [Space] public AudioSource audioSource;
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

        if (nearestEnemy != null && shortestDistance <= towerInformation.range)
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
        {
            if (fireCountdown > 0)
            {
                fireCountdown -= Time.deltaTime;
            }

            return;
        }

        // Seek enemies, Target lock on;
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * towerInformation.turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);


        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / towerInformation.fireRate;
        }

        // Every second by 1
        fireCountdown -= Time.deltaTime;
    }


    void Shoot()
    {
        GameObject arrowGO = (GameObject)Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        audioSource.Play();
        Arrow arrow = arrowGO.GetComponent<Arrow>();
        arrow.setDamage(towerInformation.damage);
        if (arrow != null)
        {
            arrow.Seek(target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerInformation.range);
    }

    private void OnMouseDown()
    {
        GameManager.instance.handleInformationUI(gameObject.transform, towerInformation);
    }

    public bool LevelUp()
    {
        PlayerStatisticsManager manager = PlayerStatisticsManager.instance;
        if (manager.Coins >= towerInformation.upgradeCost)
        {
            towerInformation.level++;
            manager.addCoins(-towerInformation.upgradeCost);
            towerInformation.damage += 5f;
            towerInformation.upgradeCost++;
            return true;
        }
        return false;
    }

    public TowerInformation TowerInformation
    {
        get => towerInformation;
        set => towerInformation = value;
    }
}