using UnityEngine;

public class UnlockedTile : MonoBehaviour
{
    [SerializeField] private int ID; // Different tiles must have unique IDs.
    [SerializeField] public ExpandDirections expandDirections;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        // Find waypoints and send it to repository of waypoints.
        // GetChild(1) is for getting parent object for waypoints
        for (int i = 0; i < gameObject.transform.GetChild(1).childCount; i++)
        {
            Waypoints.addWaypoint(gameObject.transform.GetChild(1).GetChild(i).transform);
        }

        // Waypoints added so change gamestate.
        gameManager.changeGamestate(Gamestate.StartSpawningWave);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int getID()
    {
        return ID;
    }
}