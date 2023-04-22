using System.Collections.Generic;
using UnityEngine;

public class NeighbourValueGenerator
{
    public enum NeighbourTile
    {
        Left,
        Right,
        Up,
        Down,
        Empty
    };

    private List<Vector2Int> pathCells;

    public NeighbourValueGenerator(List<Vector2Int> pathCells)
    {
        this.pathCells = pathCells;
    }

    public int DetermineStartValue(PathCellObject pathCellObject, int x, int y)
    {
        switch (FindNeighbourTile(x, y))
        {
            case NeighbourTile.Down:
                return pathCellObject.downValue.start;
            case NeighbourTile.Up:
                return pathCellObject.upValue.start;
            case NeighbourTile.Left:
                return pathCellObject.leftValue.start;
            case NeighbourTile.Right:
                return pathCellObject.rightValue.start;
        }

        return 0;
    }

    public int DetermineEndValue(PathCellObject pathCellObject, int x, int y)
    {
        switch (FindNeighbourTile(x, y))
        {
            case NeighbourTile.Down:
                return pathCellObject.downValue.end;
            case NeighbourTile.Up:
                return pathCellObject.upValue.end;
            case NeighbourTile.Left:
                return pathCellObject.leftValue.end;
            case NeighbourTile.Right:
                return pathCellObject.rightValue.end;
        }

        return 0;
    }
    
    private NeighbourTile FindNeighbourTile(int x, int y)
    {
        if (CellIsTaken(x, y - 1))
        {
            return NeighbourTile.Down;
        }

        if (CellIsTaken(x - 1, y))
        {
            return NeighbourTile.Left;
        }

        if (CellIsTaken(x + 1, y))
        {
            return NeighbourTile.Right;
        }

        if (CellIsTaken(x, y + 1))
        {
            return NeighbourTile.Up;
        }

        return NeighbourTile.Empty;
    }

    private bool CellIsTaken(int x, int y)
    {
        return pathCells.Contains(new Vector2Int(x, y));
    }
}