using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class TileGeneration : MonoBehaviour
{
    public List<GameObject> tiles = new List<GameObject>();

    public GameObject buttonTile;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] loadedTiles = Resources.LoadAll("Tiles", typeof(GameObject)).Cast<GameObject>().ToArray();
        // var guids = AssetDatabase.FindAssets("t:prefab",
        //     new[]
        //     {
        //         "Assets/Prefabs/Tilemaps/Left-Left", "Assets/Prefabs/Tilemaps/Left-Up",
        //         "Assets/Prefabs/Tilemaps/Right-Right", "Assets/Prefabs/Tilemaps/Right-Up",
        //         "Assets/Prefabs/Tilemaps/Up-Up", "Assets/Prefabs/Tilemaps/Up-Right", "Assets/Prefabs/Tilemaps/Up-Left"
        //     });
        // foreach (var guid in guids)
        // {
        //     var path = AssetDatabase.GUIDToAssetPath(guid);
        //     var tile = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        //     tiles.Add(tile);
        // }
        foreach (var tile in loadedTiles)
        {
            tiles.Add(tile);
        }
    }

    public List<GameObject> FindTilesByType(TileType[] types)
    {
        return tiles.FindAll(tile => types.Contains(tile.GetComponent<Tile>().Type));
    }

    public void GenerateTile(TileType[] possibleTileTypes, Transform buttonTileTransform)
    {
        List<GameObject> tilesList = FindTilesByType(possibleTileTypes);
        var random = new Random();
        int index = random.Next(tilesList.Count);
        GameObject tile = tilesList[index];

        GameObject tileObject = Instantiate(tile,
            new Vector3(buttonTileTransform.position.x, 0f, buttonTileTransform.position.z),
            tile.gameObject.transform.rotation);
        GenerateButtonTile(tileObject);
    }

    private void GenerateButtonTile(GameObject generatedTile)
    {
        Vector3 buttonTilePosition = DetermineButtonTilePosition(generatedTile);
        GameObject tileObject = Instantiate(buttonTile,
            buttonTilePosition,
            Quaternion.identity);
        TileType[] determinedTypes = DeterminePossibleTileTypes(generatedTile.GetComponent<Tile>().Type);
        if (determinedTypes == null)
        {
            Debug.LogError("Can't determine possible Button Types");
        }

        tileObject.GetComponent<ButtonTile>().possibleTiles = determinedTypes;
    }

    private Vector3 DetermineButtonTilePosition(GameObject generatedTile)
    {
        TileType type = generatedTile.GetComponent<Tile>().Type;
        Vector3 position = generatedTile.transform.position;
        if (type == TileType.LeftLeft || type == TileType.UpLeft)
        {
            return new Vector3(position.x - 14f, 0f, position.z);
        }

        if (type == TileType.LeftUp || type == TileType.RightUp || type == TileType.UpUp)
        {
            return new Vector3(position.x, 0f, position.z + 14f);
        }

        if (type == TileType.RightRight || type == TileType.UpRight)
        {
            return new Vector3(position.x + 14f, 0f, position.z);
        }

        Debug.Log("This type is not implemented");
        return new Vector3();
    }

    private TileType[] DeterminePossibleTileTypes(TileType type)
    {
        if (type == TileType.LeftLeft || type == TileType.UpLeft)
        {
            return new[] { TileType.LeftLeft, TileType.LeftUp };
        }

        if (type == TileType.LeftUp || type == TileType.RightUp || type == TileType.UpUp)
        {
            return new[] { TileType.UpLeft, TileType.UpRight, TileType.UpUp };
        }

        if (type == TileType.RightRight || type == TileType.UpRight)
        {
            return new[] { TileType.RightRight, TileType.RightUp };
        }

        return null;
    }
}