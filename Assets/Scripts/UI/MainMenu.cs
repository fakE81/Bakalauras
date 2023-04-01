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
    public TowersUpgradeUI towersUpgradeUI;

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
        towersUpgradeUI.UpdateText(playerTowersManager.UnitBlueprints);
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

    public void OpenMainMenuFromUpgrades()
    {
        upgradesMenu.SetActive(false);
        mainMenu.SetActive(true);
        playerTowersManager.SaveData();
        playerStatisticsManager.SaveData();
    }
    
    public void OpenMainMenuFromMapSelect()
    {
        mapSelectMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void GameExit()
    {
        // Save some data:
        playerTowersManager.SaveData();
        playerStatisticsManager.SaveData();
        Application.Quit();
    }

    public void LevelUp(int index)
    {
        towersUpgradeUI.UpdateText(playerTowersManager.UnitBlueprints);
        playerTowersManager.LevelUp(index);
        coinsText.text = playerStatisticsManager.Coins + "c";
    }
}