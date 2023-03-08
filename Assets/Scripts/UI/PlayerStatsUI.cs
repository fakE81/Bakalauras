using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Text moneyText;
    public Text waveCountText;
    public Text enemiesCountText;
    public Text healthText;


    void Update()
    {
        moneyText.text = "$" + PlayerStats.Money.ToString();
        waveCountText.text = "Wave:" + PlayerStats.Wave.ToString();
        enemiesCountText.text = "Enemies Left:" + WaveSpawner.enemiesCount.ToString();
        healthText.text = "Health:" + PlayerStats.CastleHealth.ToString();
    }
}