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
        Waypoints.reversedWaypoints = new List<Transform>();
        Waypoints.waypoints = new List<Transform>();
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        // Need to update memory:
        Waypoints.reversedWaypoints = new List<Transform>();
        Waypoints.waypoints = new List<Transform>();

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}