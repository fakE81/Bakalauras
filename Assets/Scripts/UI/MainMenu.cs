using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public GameObject mainMenu;
    public GameObject upgradesMenu;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenUpgradesMenu()
    {
        mainMenu.SetActive(false);
        upgradesMenu.SetActive(true);
    }

    public void OpenMainMenu()
    {
        upgradesMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}