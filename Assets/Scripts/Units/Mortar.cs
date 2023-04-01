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
}
