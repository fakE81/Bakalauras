using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType type;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        // Find waypoints and send it to repository of waypoints.
        // GetChild(1) is for getting parent object for waypoints
        List<Transform> waypoints = new List<Transform>();
        for (int i = 0; i < gameObject.transform.GetChild(1).childCount; i++)
        {
            waypoints.Add(gameObject.transform.GetChild(1).GetChild(i).transform);
        }

        waypoints = waypoints.OrderBy((pos) => (pos.position - new Vector3(0, 1.7f, -16f)).sqrMagnitude).ToList();
        waypoints.ForEach(waypoint => Waypoints.addWaypoint(waypoint));
        // Waypoints added so change gamestate.
        Waypoints.ReverseWaypoints();
        gameManager.changeGamestate(Gamestate.StartedWave);
    }

    public TileType Type
    {
        get => type;
        set => type = value;
    }
}