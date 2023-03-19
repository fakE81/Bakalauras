using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    public GameObject mainMenu;
    public GameObject upgradesMenu;
    public GameObject mapSelectMenu;
    public Text coinsText;
    public Text banditsHighScore;
    private PlayerStatisticsManager playerStatisticsManager;
    private PlayerTowersManager playerTowersManager;

    private void Start()
    {
        playerStatisticsManager = PlayerStatisticsManager.instance;
        playerTowersManager = PlayerTowersManager.instance;
    }

    public void StartBanditMap()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenUpgradesMenu()
    {
        coinsText.text = playerStatisticsManager.Coins + "c";
        mainMenu.SetActive(false);
        upgradesMenu.SetActive(true);
    }

    public void OpenMapSelectMenu()
    {
        banditsHighScore.text = "Highscore: " + playerStatisticsManager.BanditsHighscore; 
        mainMenu.SetActive(false);
        mapSelectMenu.SetActive(true);
    }

    public void OpenMainMenu()
    {
        upgradesMenu.SetActive(false);
        mapSelectMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void LevelUp(int index)
    {
        playerTowersManager.LevelUp(index);
        coinsText.text = playerStatisticsManager.Coins + "c";
    }
}