using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContainer : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Vector2Int> pathCells;
    private GameManager gameManager;
    private bool unlocked = false;

    private void Update()
    {
        if (unlocked)
        {
            // Move Tile block:
            float newY = transform.position.y + Time.deltaTime * 1.5f;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            if (transform.position.y >= 0)
            {
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                unlocked = false;
                gameManager.changeGamestate(Gamestate.StartedWave);
            }
        }
    }

    public void UnlockTiles()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        var demo = GameObject.FindWithTag("GameController").GetComponent<GridManagerDEMO>();
        if (demo != null)
        {
            demo.ShowUnlockButton();
        }
        else
        {
            var other = GameObject.FindWithTag("GameController").GetComponent<GridManager>();
            
        }
        
        gameManager = GameManager.instance;
        // TODO: Send pathCell coordinates to waypoint manager for enemies
        foreach (var pathCell in pathCells)
        {
            Waypoints.addWaypoint(new Vector3(pathCell.x, 0.17f, pathCell.y));
        }
        Waypoints.ReverseWaypoints();
        // gameManager.changeGamestate(Gamestate.StartedWave);
        transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
        unlocked = true;
    }
}
