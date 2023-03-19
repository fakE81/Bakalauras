using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatisticsManager : MonoBehaviour
{
    public static PlayerStatisticsManager instance;
    private int coins = 0;
    private int banditsHighscore = 0;
    void Start()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void addCoins(int coins)
    {
        this.coins += coins;
    }

    public int Coins => coins;

    public int BanditsHighscore
    {
        get => banditsHighscore;
        set => banditsHighscore = value;
    }
}
