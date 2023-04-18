using TMPro;
using UnityEngine;

public class InformationUI : MonoBehaviour
{
    [Header("Information Texts")] public TextMeshProUGUI damageText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI turnSpeedText;
    public Transform rangeUI;
    public Transform cameraTransform;

    public void updateInformation(Transform pos, TowerInformation towerInformation)
    {
        transform.position = new Vector3(pos.position.x, 3.4f, pos.position.z);
        // Calculate the rotation angle around the Y-axis
        Vector3 direction = cameraTransform.position - transform.position;
        direction.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.Rotate(new Vector3(0f, 180f, 0f));
        
        damageText.text = "Damage: " + towerInformation.damage;
        rangeText.text = "Range: " + towerInformation.range;
        fireRateText.text = "Firerate: " + towerInformation.fireRate;
        turnSpeedText.text = "Turn speed:" + towerInformation.turnSpeed;
        rangeUI.localScale = new Vector3(towerInformation.range * 2, 0.01f, towerInformation.range * 2);
    }
}