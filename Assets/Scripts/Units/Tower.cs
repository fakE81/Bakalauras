using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected TowerInformation towerInformation;
    [SerializeField] protected GameObject AudioPrefab;
    [SerializeField] protected GameObject arrowPrefab;
    [SerializeField] protected Transform firePoint;

    protected Transform target;
    protected float fireCountdown = 0f;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    public virtual bool LevelUp()
    {
        PlayerStatisticsManager manager = PlayerStatisticsManager.instance;
        if (manager.Coins >= towerInformation.upgradeCost)
        {
            towerInformation.level++;
            manager.addCoins(-towerInformation.upgradeCost);
            towerInformation.damage += 2f;
            towerInformation.upgradeCost+=towerInformation.level;
            return true;
        }

        return false;
    }

    public TowerInformation TowerInformation
    {
        get => towerInformation;
        set => towerInformation = value;
    }

    protected virtual void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (shortestDistance > distanceToEnemy)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
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

    protected virtual void Shoot()
    {
        GameObject projectileGameObject = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        IProjectile projectile = projectileGameObject.GetComponent<IProjectile>();
        if (projectile != null)
        {
            projectile.Seek(target, towerInformation.damage);
        }

        Instantiate(AudioPrefab, target.position, Quaternion.identity);
    }

    protected virtual void TowerRotation()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion
            .Lerp(transform.rotation, lookRotation, Time.deltaTime * TowerInformation.turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
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

    public string Name
    {
        get => name;
        set => name = value;
    }
}