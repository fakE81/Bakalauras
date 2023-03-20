using TMPro;
using UnityEngine;

public class InformationUI : MonoBehaviour
{
    [Header("Information Texts")] public TextMeshProUGUI damageText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI turnSpeedText;
    public Transform rangeUI;

    public void updateInformation(Transform pos, TowerInformation towerInformation)
    {
        transform.position = new Vector3(pos.position.x, 3.4f, pos.position.z);

        damageText.text = "Damage: " + towerInformation.damage;
        rangeText.text = "Range: " + towerInformation.range;
        fireRateText.text = "Firerate: " + towerInformation.fireRate;
        turnSpeedText.text = "Turn speed:" + towerInformation.turnSpeed;
        rangeUI.localScale = new Vector3(towerInformation.range * 2, 0.01f, towerInformation.range * 2);
    }
}