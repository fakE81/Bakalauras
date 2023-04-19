using System.Collections;
using UnityEngine;

public class MineTower : Tower
{
    [SerializeField] private LayerMask layerMask;

    private void Update()
    {
        if (fireCountdown > 0)
        {
            fireCountdown -= Time.deltaTime;
        }

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / TowerInformation.fireRate;
        }

        // Every second by 1
        fireCountdown -= Time.deltaTime;
    }

    protected override void UpdateTarget()
    {
        // Do nothing
    }

    protected override void Shoot()
    {
        StartCoroutine(SpawnMines());
    }

    private IEnumerator SpawnMines()
    {
        Collider[] hitColliders =
            Physics.OverlapSphere(gameObject.transform.position, towerInformation.range, layerMask);
        foreach (var collider in hitColliders)
        {
            if (!collider.GetComponent<Node>().hasMine)
            {
                GameObject projectileGameObject = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
                IProjectile projectile = projectileGameObject.GetComponent<IProjectile>();
                if (projectile != null)
                {
                    projectile.Seek(collider.transform, towerInformation.damage);
                }

                collider.GetComponent<Node>().hasMine = true;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}