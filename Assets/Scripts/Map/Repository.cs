using System.Collections.Generic;
using UnityEngine;

public class Repository
{
    private const float TILE_SIZE = 10f;

    public List<GameObject> tiles = new List<GameObject>();

    public void addTile(GameObject tile)
    {
        tiles.Add(tile);
    }

    public void removeTile(GameObject tile)
    {
        tiles.Remove(tile);
    }

    public List<GameObject> findTilesByCoordinates(float x, float z)
    {
        return tiles.FindAll(gameobject =>
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

    public List<GameObject> findLeftTiles(float x, float z)
    {
        return tiles.FindAll(gameobject =>
            (gameobject.transform.position.x == x - TILE_SIZE && gameobject.transform.position.z == z));
    }

    public List<GameObject> findRightTiles(float x, float z)
    {
        return tiles.FindAll(gameobject =>
            (gameobject.transform.position.x == x + TILE_SIZE && gameobject.transform.position.z == z));
    }

    public List<GameObject> findUpTiles(float x, float z)
    {
        return tiles.FindAll(gameobject =>
            (gameobject.transform.position.x == x && gameobject.transform.position.z == z + TILE_SIZE));
    }

    public List<GameObject> findDownTiles(float x, float z)
    {
        return tiles.FindAll(gameobject =>
            (gameobject.transform.position.x == x && gameobject.transform.position.z == z - TILE_SIZE));
    }
}