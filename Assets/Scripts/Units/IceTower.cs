using UnityEngine;

public class IceTower : Tower
{

    private void Update()
    {
        if (target == null)
        {
            if (fireCountdown > 0)
            {
                fireCountdown -= Time.deltaTime;
            }

            return;
        }
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / TowerInformation.fireRate;
        }

        // Every second by 1
        fireCountdown -= Time.deltaTime;
    }
    
    protected override void Shoot()
    {
        GameObject projectileGameObject = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        IProjectile projectile = projectileGameObject.GetComponent<IProjectile>();
        if (projectile != null)
        {
            projectile.Seek(target, towerInformation.slow);
        }
        audioSource.Play();
    }
}
