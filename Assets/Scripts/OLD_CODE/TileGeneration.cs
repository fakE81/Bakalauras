using System.Collections.Generic;
using UnityEngine;

public class TileGeneration : MonoBehaviour
{
    const int TILE_SPACES = 2;

    [Header("Tile Prefabs")] [SerializeField]
    private GameObject castleBlock;

    [SerializeField] private GameObject dirtBlock;
    [SerializeField] private GameObject grassBlock;
    [SerializeField] private GameObject waypointObject;
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    private GameObject parentObject;
    private GameObject blocksParent;
    private GameObject waypointsParentObject;

    [Header("Generation Settings")] [SerializeField]
    int spread; // How many blocks can go.

    [SerializeField] int maxProbability; // How many blocks can go.
    [SerializeField] int probability; // How many blocks can go.
    List<Vector2> usedCoordinates = new List<Vector2>();
    List<int> movedTiles = new List<int> { 0, 0, 0, 0 }; // Holds how many blocks generation went.

    void Start()
    {
        parentObject = new GameObject("Start_Tile");
        waypointsParentObject = new GameObject("Waypoints");
        blocksParent = new GameObject("_blocks");
        startTileGeneration();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void startTileGeneration()
    {
        // Generate first block (Castle):
        Instantiate(castleBlock, new Vector3(0f, 0f, 0f), Quaternion.identity).transform
            .SetParent(blocksParent.transform);
        usedCoordinates.Add(new Vector2(0f, 0f));
        // Generate whole block and add it to the parent GameObject:
        generate();
        waypointsParentObject.transform.SetParent(parentObject.transform);
        blocksParent.transform.SetParent(parentObject.transform);
        //MapDatabase.addGameobject(parentObject);
    }


    private void generate()
    {
        int direction = Random.Range(0, 4);
        if (direction == 0)
        {
            generateRoad(0, 0, 2);
        }
        else if (direction == 1)
        {
            generateRoad(1, 0, -2);
        }
        else if (direction == 2)
        {
            generateRoad(2, 2, 0);
        }
        else
        {
            generateRoad(3, -2, 0);
        }

        generateGrass();
        generateWaypoints();
    }

    private void generateRoad(int restriction, int x, int y)
    {
        int positionX = x;
        int positionZ = y;
        int lastPositionX = positionX;
        int lastPositionZ = positionZ;
        int goal = endGoal(restriction);

        //Create first road tile:
        usedCoordinates.Add(new Vector2(positionX, positionZ));
        Instantiate(dirtBlock, new Vector3(positionX, 0, positionZ), Quaternion.identity).transform
            .SetParent(blocksParent.transform);
        Instantiate(endPoint, new Vector3(positionX, 2, positionZ), Quaternion.identity).transform
            .SetParent(parentObject.transform);
        while (movedTiles[goal] < spread - 1)
        {
            int direction = generateDirection(goal); // 0 - Down, 1 - Up, 2 - Left,3 - Rigth
            if (direction != restriction)
            {
                switch (direction)
                {
                    case 0:
                        positionZ -= TILE_SPACES;
                        break;
                    case 1:
                        positionZ += TILE_SPACES;
                        break;
                    case 2:
                        positionX -= TILE_SPACES;
                        break;
                    case 3:
                        positionX += TILE_SPACES;
                        break;
                }

                Vector2 position = new Vector2(positionX, positionZ);
                if (checkIfTileCanBeAdded(direction, position))
                {
                    Instantiate(dirtBlock, new Vector3(position.x, 0, position.y), Quaternion.identity).transform
                        .SetParent(blocksParent.transform);

                    usedCoordinates.Add(position);
                    updateMovedTiles(direction);

                    lastPositionX = positionX;
                    lastPositionZ = positionZ;
                    //Debug.Log("Tile was added:" +positionX + " " +positionZ + "Direction:" + direction);
                }
                else
                {
                    positionX = lastPositionX;
                    positionZ = lastPositionZ;
                    //Debug.LogWarning("Tile was not added:" +positionX + " " +positionZ);
                }
            }
        }

        Instantiate(startPoint, new Vector3(positionX, 2, positionZ), Quaternion.identity).transform
            .SetParent(parentObject.transform);
    }

    private int generateDirection(int goal)
    {
        int number = Random.Range(0, maxProbability);
        if (0 <= number && number <= probability)
        {
            if (goal == 0 || goal == 1)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
        else if (probability < number && number <= probability * 2)
        {
            if (goal == 0 || goal == 1)
            {
                return 3;
            }
            else
            {
                return 1;
            }
        }

        return goal;
    }

    private void generateGrass()
    {
        float startPointX = (TILE_SPACES * spread);
        float startPointZ = (TILE_SPACES * spread);
        for (int i = 0; i < (spread * TILE_SPACES) + 1; i++)
        {
            float z = startPointZ;
            for (int j = 0; j < (spread * TILE_SPACES) + 1; j++)
            {
                if (!usedCoordinates.Contains(new Vector2(startPointX, z)))
                {
                    Instantiate(grassBlock, new Vector3(startPointX, 0, z), Quaternion.identity).transform
                        .SetParent(blocksParent.transform);
                }
                else
                {
                    //Debug.Log(startPointX + " " + z);
                    //usedCoordinates.ForEach(item => Debug.Log("Used:" + item.ToString()));
                }

                z -= TILE_SPACES;
            }

            startPointX -= TILE_SPACES;
        }
    }

    private void generateWaypoints()
    {
        for (int i = 0; i < usedCoordinates.Count - 2; i++)
        {
            //Debug.Log("Coordinates1:" + usedCoordinates[i].x + ";" + usedCoordinates[i].y + " Coordinates2:" + usedCoordinates[i+1].x + ";" + usedCoordinates[i+1].y);
            if (checkIfWaypointsNeeded(i, i + 1, i + 2))
            {
                Instantiate(waypointObject, new Vector3(usedCoordinates[i + 1].x, 1.7f, usedCoordinates[i + 1].y),
                    Quaternion.identity).transform.SetParent(waypointsParentObject.transform);
            }
        }
    }

    private bool checkIfWaypointsNeeded(int index1, int index2, int index3)
    {
        Vector2 first = usedCoordinates[index1];
        Vector2 second = usedCoordinates[index2];
        Vector2 third = usedCoordinates[index3];
        if ((first.x == second.x - 2 && second.x == third.x - 2) ||
            (first.x == second.x + 2 && second.x == third.x + 2))
        {
            return false;
        }
        else if ((first.y == second.y - 2 && second.y == third.y - 2) ||
                 (first.y == second.y + 2 && second.y == third.y + 2))
        {
            return false;
        }

        return true;
    }

    private void updateMovedTiles(int direction)
    {
        if (direction == 0)
        {
            movedTiles[0]++;
            movedTiles[1]--;
        }
        else if (direction == 1)
        {
            movedTiles[0]--;
            movedTiles[1]++;
        }
        else if (direction == 2)
        {
            movedTiles[2]++;
            movedTiles[3]--;
        }
        else
        {
            movedTiles[2]--;
            movedTiles[3]++;
        }
    }

    private bool checkIfTileCanBeAdded(int direction, Vector2 position)
    {
        bool canBeAdded = true;
        if (usedCoordinates.Contains(position))
        {
            return false;
        }

        switch (direction)
        {
            case 0: // Down
                if (
                    usedCoordinates.Contains(new Vector2(position.x + TILE_SPACES, position.y))
                    ||
                    usedCoordinates.Contains(new Vector2(position.x - TILE_SPACES, position.y))
                    ||
                    usedCoordinates.Contains(new Vector2(position.x, position.y - TILE_SPACES))
                    ||
                    movedTiles[0] > spread - 1
                )
                {
                    return false;
                }

                break;
            case 1: // Up
                if (
                    usedCoordinates.Contains(new Vector2(position.x + TILE_SPACES, position.y))
                    ||
                    usedCoordinates.Contains(new Vector2(position.x - TILE_SPACES, position.y))
                    ||
                    usedCoordinates.Contains(new Vector2(position.x, position.y + TILE_SPACES))
                    ||
                    movedTiles[1] > spread - 1)
                {
                    return false;
                }

                break;
            case 2: // Left
                if (
                    usedCoordinates.Contains(new Vector2(position.x - TILE_SPACES, position.y))
                    ||
                    usedCoordinates.Contains(new Vector2(position.x, position.y - TILE_SPACES))
                    ||
                    usedCoordinates.Contains(new Vector2(position.x, position.y + TILE_SPACES))
                    ||
                    movedTiles[2] > spread - 1)
                {
                    return false;
                }

                break;
            case 3: // Right
                if (
                    usedCoordinates.Contains(new Vector2(position.x + TILE_SPACES, position.y))
                    ||
                    usedCoordinates.Contains(new Vector2(position.x, position.y - TILE_SPACES))
                    ||
                    usedCoordinates.Contains(new Vector2(position.x, position.y + TILE_SPACES))
                    ||
                    movedTiles[3] > spread - 1)
                {
                    return false;
                }

                break;
        }

        return canBeAdded;
    }

    private int endGoal(int restriction)
    {
        if (restriction == 0)
        {
            return 1;
        }
        else if (restriction == 1)
        {
            return 0;
        }
        else if (restriction == 2)
        {
            return 3;
        }

        return 2;
    }
}