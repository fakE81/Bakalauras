using System;

[Serializable]
public class TowerInformation
{
    public String towerName;
    public float range = 5f;
    public float fireRate = 1f;
    public float damage = 10f;
    public float turnSpeed = 3f;
    public int level = 1;
    public int upgradeCost = 2;
    public float slow = 0.1f;

    public TowerInformation()
    {
    }
}
