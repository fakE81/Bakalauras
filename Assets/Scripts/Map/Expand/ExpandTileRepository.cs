using System.Collections.Generic;
using UnityEngine;

public class ExpandTileRepository : MonoBehaviour
{
    private const float TILE_SIZE = 10f;

    public static List<GameObject> mapBlocks = new List<GameObject>();

    public static GameObject findGameobjectByName(string name)
    {
        return mapBlocks.Find(gameobject => gameobject.name.Equals(name));
    }

    public static void addGameobject(GameObject gameObject)
    {
        mapBlocks.Add(gameObject);
    }

    public static void removeGameobject(GameObject gameObject)
    {
        mapBlocks.Remove(gameObject);
    }

    public static List<GameObject> findTilesByCoordinates(float x, float z)
    {
        return mapBlocks.FindAll(gameobject =>
            ((gameobject.transform.position.x == x - TILE_SIZE && gameobject.transform.position.z == z)
             ||
             (gameobject.transform.position.x == x && gameobject.transform.position.z == z - TILE_SIZE)
             ||
             (gameobject.transform.position.x == x + TILE_SIZE && gameobject.transform.position.z == z)
             ||
             (gameobject.transform.position.x == x && gameobject.transform.position.z == z + TILE_SIZE))
            &&
            gameobject.activeSelf == false);
    }

    public static List<GameObject> findLeftTiles(float x, float z)
    {
        return mapBlocks.FindAll(gameobject =>
            (gameobject.transform.position.x == x - TILE_SIZE && gameobject.transform.position.z == z));
    }

    public static List<GameObject> findRightTiles(float x, float z)
    {
        return mapBlocks.FindAll(gameobject =>
            (gameobject.transform.position.x == x + TILE_SIZE && gameobject.transform.position.z == z));
    }

    public static List<GameObject> findUpTiles(float x, float z)
    {
        return mapBlocks.FindAll(gameobject =>
            (gameobject.transform.position.x == x && gameobject.transform.position.z == z + TILE_SIZE));
    }

    public static List<GameObject> findDownTiles(float x, float z)
    {
        return mapBlocks.FindAll(gameobject =>
            (gameobject.transform.position.x == x && gameobject.transform.position.z == z - TILE_SIZE));
    }
}