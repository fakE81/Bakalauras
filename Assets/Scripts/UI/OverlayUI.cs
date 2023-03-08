using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayUI : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    public void ExitGame()
    {
        UnlockedTilesRepository.unlockedTiles = new List<GameObject>();
        Waypoints.reversedWaypoints = new List<Transform>();
        Waypoints.waypoints = new List<Transform>();
        ExpandTileRepository.mapBlocks = new List<GameObject>();
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        // Need to update memory:
        UnlockedTilesRepository.unlockedTiles = new List<GameObject>();
        Waypoints.reversedWaypoints = new List<Transform>();
        Waypoints.waypoints = new List<Transform>();
        ExpandTileRepository.mapBlocks = new List<GameObject>();

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


    public void SelectPower(string msg)
    {
        gm.SelectedPower(msg);
    }
}