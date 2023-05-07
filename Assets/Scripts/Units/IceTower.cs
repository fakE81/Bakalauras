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

        Instantiate(AudioPrefab, gameObject.transform.position, Quaternion.identity);
    }

    public override bool LevelUp()
    {
        PlayerStatisticsManager manager = PlayerStatisticsManager.instance;
        if (manager.Coins >= towerInformation.upgradeCost)
        {
            towerInformation.level++;
            manager.addCoins(-towerInformation.upgradeCost);
            towerInformation.slow += 2;
            towerInformation.upgradeCost+=towerInformation.level;
            return true;
        }

        return false;
    }
}
