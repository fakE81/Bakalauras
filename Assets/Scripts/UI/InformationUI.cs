using TMPro;
using UnityEngine;

public class InformationUI : MonoBehaviour
{
    [Header("Information Texts")] public TextMeshProUGUI damageText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI turnSpeedText;
    public Transform rangeUI;

    public void updateInformation(Transform pos, float damage, float range, float fireRate, float turnSpeed)
    {
        transform.position = new Vector3(pos.position.x, 3.4f, pos.position.z);

        damageText.text = "Damage: " + damage;
        rangeText.text = "Range: " + range;
        fireRateText.text = "Firerate: " + fireRate;
        turnSpeedText.text = "Turn speed:" + turnSpeed;
        rangeUI.localScale = new Vector3(range * 2, 0.01f, range * 2);
    }
}