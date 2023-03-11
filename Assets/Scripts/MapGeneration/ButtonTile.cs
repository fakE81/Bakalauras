using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTile : MonoBehaviour
{
    public Button expandButton;
    public TileType[] possibleTiles;
    [SerializeField]
    private TileGeneration tileGeneration;

    public TileType[] PossibleTiles
    {
        get => possibleTiles;
        set => possibleTiles = value;
    }

    public void Start()
    {
        tileGeneration = GameObject.FindGameObjectWithTag("MapGeneration").GetComponent<TileGeneration>();
    }

    public void Unlock()
    {
        //Button pressed to unlock.
        tileGeneration.GenerateTile(possibleTiles, this.transform);
        Destroy(gameObject);
    }
}
