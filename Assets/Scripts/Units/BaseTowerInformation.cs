using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName="BaseTowerInformation", menuName = "Towers/BaseInformation")]
public class BaseTowerInformation : ScriptableObject
{
    public string towerName;
    public float range;
    public float fireRate;
    public float damage;
    public float turnSpeed;
    public int level;
    public int upgradeCost;
    public int slow;
}
