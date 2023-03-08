using UnityEngine;

public class ExpandTileGenerator : MonoBehaviour
{
    private const float tileSize = 10f;

    [SerializeField] private GameObject map_tile;

    public void addExpandButtons(int id, Vector3 position)
    {
        //From id determine where buttons can be:
        ExpandDirections expandDirections = getDirections(id, position);
        // Instantiate buttons:
        // And check if there is no unlocked tile
        UnlockedTilesRepository.findTilesByCoordinates(position.x, position.z);
        if (expandDirections.getUp() && !isUpTileExists(position))
        {
            Debug.Log("Up");
            ExpandTileRepository.addGameobject(Instantiate(map_tile,
                new Vector3(position.x, position.y, position.z + tileSize), Quaternion.identity));
        }

        if (expandDirections.getDown() && !isDownTileExists(position))
        {
            Debug.Log("Down");
            ExpandTileRepository.addGameobject(Instantiate(map_tile,
                new Vector3(position.x, position.y, position.z - tileSize), Quaternion.identity));
        }

        if (expandDirections.getLeft() && !isLeftTileExists(position))
        {
            Debug.Log("Left");
            ExpandTileRepository.addGameobject(Instantiate(map_tile,
                new Vector3(position.x - tileSize, position.y, position.z), Quaternion.identity));
        }

        if (expandDirections.getRight() && !isRightTileExists(position))
        {
            Debug.Log("Right");
            ExpandTileRepository.addGameobject(Instantiate(map_tile,
                new Vector3(position.x + tileSize, position.y, position.z), Quaternion.identity));
        }
    }

    private bool isLeftTileExists(Vector3 position)
    {
        return UnlockedTilesRepository.findLeftTiles(position.x, position.z).Count == 1 ||
               ExpandTileRepository.findLeftTiles(position.x, position.z).Count == 1;
    }

    private bool isRightTileExists(Vector3 position)
    {
        return UnlockedTilesRepository.findRightTiles(position.x, position.z).Count == 1 ||
               ExpandTileRepository.findRightTiles(position.x, position.z).Count == 1;
    }

    private bool isUpTileExists(Vector3 position)
    {
        return UnlockedTilesRepository.findUpTiles(position.x, position.z).Count == 1 ||
               ExpandTileRepository.findUpTiles(position.x, position.z).Count == 1;
    }

    private bool isDownTileExists(Vector3 position)
    {
        return UnlockedTilesRepository.findDownTiles(position.x, position.z).Count == 1 ||
               ExpandTileRepository.findDownTiles(position.x, position.z).Count == 1;
    }

    private ExpandDirections getDirections(int id, Vector3 position)
    {
        ExpandDirections directions = new ExpandDirections();
        switch (id)
        {
            case 0:
            {
                if (isRightTileExists(position))
                {
                    directions.setDirections(false, true, false, false);
                    break;
                }
                else
                {
                    directions.setDirections(true, false, false, false);
                    break;
                }
            }
            case 1:
                directions.setDirections(false, false, true, false);
                break;
            case 2:
                directions.setDirections(true, true, true, false);
                break;
            case 3:
                directions.setDirections(false, true, false, false);
                break;
            case 4:
                directions.setDirections(false, false, true, false);
                break;
            case 5:
                directions.setDirections(false, false, true, false);
                break;
            case 6:
                directions.setDirections(true, false, false, false);
                break;
            case 7:
                directions.setDirections(true, false, false, false);
                break;
        }

        return directions;
    }
}