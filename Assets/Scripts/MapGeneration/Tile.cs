using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private TileType type;

    public TileType Type
    {
        get => type;
        set => type = value;
    }
}