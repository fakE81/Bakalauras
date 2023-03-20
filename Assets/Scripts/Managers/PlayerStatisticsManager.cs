using System.IO;
using UnityEngine;

public class PlayerStatisticsManager : MonoBehaviour
{
    public static PlayerStatisticsManager instance;
    private PlayerStatisticsInformation playerStatisticsInformation;
    void Start()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
        playerStatisticsInformation = new PlayerStatisticsInformation();
        // Overwrites if some data exists:
        LoadData();
        DontDestroyOnLoad(this.gameObject);
    }

    public void addCoins(int coins)
    {
        this.playerStatisticsInformation.coins += coins;
    }

    public int Coins => playerStatisticsInformation.coins;

    public int BanditsHighscore
    {
        get => playerStatisticsInformation.banditsHighscore;
        set => playerStatisticsInformation.banditsHighscore = value;
    }
    
    public void SaveData()
    {
        string information = JsonUtility.ToJson(playerStatisticsInformation);
        File.WriteAllText(Application.persistentDataPath + "/PlayerInformation.json", information);
    }

    private void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerInformation.json"))
        {
            string information = File.ReadAllText(Application.persistentDataPath + "/PlayerInformation.json");
            PlayerStatisticsInformation playerInformation = JsonUtility.FromJson<PlayerStatisticsInformation>(information);
            playerStatisticsInformation = playerInformation;
        }
    }
}
