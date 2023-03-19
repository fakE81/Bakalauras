using UnityEngine;

public class Cheats : MonoBehaviour
{
    private bool isSpeedUp = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (isSpeedUp)
            {
                Time.timeScale = 1;
                isSpeedUp = false;
            }
            else
            {
                Time.timeScale = 3;
                isSpeedUp = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            PlayerStats.Money += 1000;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            PlayerStats.CastleHealth += 10;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            PlayerStatisticsManager.instance.addCoins(100);
        }
    }
}
