using UnityEngine;

public class TowersUpgradeUI : MonoBehaviour
{
    [SerializeField] private TowerInformationText[] informationText;


    public void UpdateText(UnitBlueprint[] blueprints)
    {
        for (int i = 0; i < blueprints.Length; i++)
        {
            UpdateTowerText(informationText[i],
                blueprints[i].prefab.transform.GetChild(0).GetComponent<Tower>().TowerInformation);
        }
    }

    private void UpdateTowerText(TowerInformationText informationText, TowerInformation towerInformation)
    {
        informationText.cost.text = towerInformation.upgradeCost + "c";
        informationText.level.text = "Level:" + towerInformation.level;
        if (towerInformation.towerName == "Ice")
        {
            informationText.damage.text = "Slow:" + towerInformation.slow * 100 + "%";
        }
        else
        {
            informationText.damage.text = "Damage:" + towerInformation.damage;
        }
        informationText.range.text = "Range:" + towerInformation.range;
        informationText.fireRate.text = "Fire Rate:" + towerInformation.fireRate + "/s";
    }
}