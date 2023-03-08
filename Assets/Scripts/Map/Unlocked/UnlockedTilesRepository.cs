using System.Collections.Generic;
using UnityEngine;

public class UnlockedTilesRepository : MonoBehaviour
{
    private const float TILE_SIZE = 10f;

    public static List<GameObject> unlockedTiles = new List<GameObject>();

    public static void addTile(GameObject tile)
    {
        unlockedTiles.Add(tile);
    }

    public static List<GameObject> findTilesByCoordinates(float x, float z)
    {
        return unlockedTiles.FindAll(gameobject =>
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
        return unlockedTiles.FindAll(gameobject =>
            (gameobject.transform.position.x == x - TILE_SIZE && gameobject.transform.position.z == z));
    }

    public static List<GameObject> findRightTiles(float x, float z)
    {
        return unlockedTiles.FindAll(gameobject =>
            (gameobject.transform.position.x == x + TILE_SIZE && gameobject.transform.position.z == z));
    }

    public static List<GameObject> findUpTiles(float x, float z)
    {
        return unlockedTiles.FindAll(gameobject =>
            (gameobject.transform.position.x == x && gameobject.transform.position.z == z + TILE_SIZE));
    }

    public static List<GameObject> findDownTiles(float x, float z)
    {
        return unlockedTiles.FindAll(gameobject =>
            (gameobject.transform.position.x == x && gameobject.transform.position.z == z - TILE_SIZE));
    }
}