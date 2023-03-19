using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Text moneyText;
    public Text waveCountText;
    public Text enemiesCountText;
    public Text healthText;
    public Text earnedCoinsText;


    void Update()
    {
        moneyText.text = "$" + PlayerStats.Money.ToString();
        waveCountText.text = "Wave:" + PlayerStats.Wave.ToString();
        enemiesCountText.text = "Enemies Left:" + WaveSpawner.enemiesCount.ToString();
        healthText.text = "Health:" + PlayerStats.CastleHealth.ToString();
        earnedCoinsText.text = PlayerStats.EARNED_COINS + "c";
    }
}