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
        var guids = AssetDatabase.FindAssets("t:prefab",
            new[]
            {
                // "Assets/Prefabs/Tilemaps/Left-Left", "Assets/Prefabs/Tilemaps/Left-Up",
                // "Assets/Prefabs/Tilemaps/Right-Right", "Assets/Prefabs/Tilemaps/Right-Up",
                "Assets/Prefabs/Tilemaps/Up-Up"
            });
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var tile = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
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
        //TODO: DeterminePossibleTilesTypes
        tileObject.GetComponent<ButtonTile>().possibleTiles = new[] { TileType.UpUp };
    }

    private Vector3 DetermineButtonTilePosition(GameObject generatedTile)
    {
        if (generatedTile.GetComponent<Tile>().Type.Equals(TileType.UpUp))
        {
            return new Vector3(generatedTile.transform.position.x, 0f, generatedTile.transform.position.z + 14f);
        }

        return new Vector3();
    }
}