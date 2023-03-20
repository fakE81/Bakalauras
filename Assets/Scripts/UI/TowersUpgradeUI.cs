using UnityEngine;
using UnityEngine.UI;

public class TowersUpgradeUI : MonoBehaviour
{
    
    public Text balistaLevel;
    public Text balistaDamage;
    public Text balistaCost;
    public Text balistaRange;
    public Text balistaFireRate;
    

    public void UpdateBalistaText(TowerInformation towerInformation)
    {
        balistaCost.text = towerInformation.upgradeCost + "c";
        balistaLevel.text = "Level:" + towerInformation.level;
        balistaDamage.text = "Damage:" + towerInformation.damage;
        balistaRange.text = "Range:" + towerInformation.range;
        balistaFireRate.text = "Fire Rate:" + towerInformation.fireRate +"/s";
    }
}
