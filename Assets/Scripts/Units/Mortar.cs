using UnityEngine;

public class Mortar : Tower
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

    public override bool LevelUp()
    {
        PlayerStatisticsManager manager = PlayerStatisticsManager.instance;
        if (manager.Coins >= towerInformation.upgradeCost)
        {
            towerInformation.level++;
            manager.addCoins(-towerInformation.upgradeCost);
            towerInformation.damage += 4f;
            towerInformation.upgradeCost+=towerInformation.level;
            return true;
        }
        return false;
    }
}
