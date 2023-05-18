using System.Collections.Generic;
using UnityEngine;

public class PathGenerator
{
    public enum Direction
    {
        Up,
        Left,
        Right,
        UpLeft,
        UpRight,
        LeftUp,
        RightUp
    };

    public int[,] grid = new int[9, 9]
    {
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };


    private int width, height;
    private List<Vector2Int> pathCells;

    public PathGenerator(int width, int height)
    {
        this.width = width;
        this.height = height;
        pathCells = new List<Vector2Int>();
    }

    public List<Vector2Int> GeneratePath(Vector2Int start, Vector2Int end, Vector2Int offset)
    {
        // Convert to grid coordinates:
        start = ConvertToGridCoordinates(start);
        end = ConvertToGridCoordinates(end);

        InitializeGrid();

        List<Vector2Int> openNodes = new List<Vector2Int>();
        List<Vector2Int> closedNodes = new List<Vector2Int>();

        openNodes.Add(start);
        Dictionary<Vector2Int, Vector2Int> parentNodes = new Dictionary<Vector2Int, Vector2Int>();

        Dictionary<Vector2Int, int> gScores = new Dictionary<Vector2Int, int>();
        gScores[start] = 0;

        Dictionary<Vector2Int, int> fScores = new Dictionary<Vector2Int, int>();
        fScores[start] = Heuristic(start, end);

        while (openNodes.Count > 0)
        {
            Vector2Int current = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (fScores.ContainsKey(openNodes[i]) && fScores[openNodes[i]] < fScores[current])
                {
                    current = openNodes[i];
                }
            }

            if (current == end)
            {
                return ReconstructPath(parentNodes, current, offset);
            }

            openNodes.Remove(current);
            closedNodes.Add(current);
            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedNodes.Contains(neighbor))
                {
                    continue;
                }

                int tentativeGScore = gScores[current] + 1;

                if (!openNodes.Contains(neighbor) || tentativeGScore < gScores[neighbor])
                {
                    parentNodes[neighbor] = current;
                    gScores[neighbor] = tentativeGScore;
                    fScores[neighbor] = tentativeGScore + Heuristic(neighbor, end);
                    if (!openNodes.Contains(neighbor))
                    {
                        openNodes.Add(neighbor);
                    }
                }
            }
        }

        // No path found
        return null;
    }

    public bool CellIsFree(int x, int y)
    {
        return !pathCells.Contains(new Vector2Int(x, y));
    }

    public bool CellIsTaken(int x, int y)
    {
        return pathCells.Contains(new Vector2Int(x, y));
    }

    public int GetCellNeighbourValue(int x, int y, Vector2Int startPos, Vector2Int endPos,
        PathCellObject[] pathCellObjects)
    {
        int returnValue = 0;
        Direction direction = DetermineDirection(startPos, endPos);
        if (startPos.Equals(new Vector2Int(x, y)))
        {
            return DetermineNeighbourValue(direction, true, x, y, pathCellObjects);
        }

        if (endPos.Equals(new Vector2Int(x, y)))
        {
            return DetermineNeighbourValue(direction, false, x, y, pathCellObjects);
        }

        if (CellIsTaken(x, y - 1))
        {
            returnValue += 1;
        }

        if (CellIsTaken(x - 1, y))
        {
            returnValue += 2;
        }

        if (CellIsTaken(x + 1, y))
        {
            returnValue += 4;
        }

        if (CellIsTaken(x, y + 1))
        {
            returnValue += 8;
        }

        return returnValue;
    }

    int Heuristic(Vector2Int current, Vector2Int end)
    {
        return Mathf.Abs(current.x - end.x) + Mathf.Abs(current.y - end.y);
    }

    List<Vector2Int> GetNeighbors(Vector2Int current)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Check for valid neighbors in grid
        if (current.x > 0 && grid[current.x - 1, current.y] == 0)
        {
            neighbors.Add(new Vector2Int(current.x - 1, current.y));
        }

        if (current.x < grid.GetLength(0) - 1 && grid[current.x + 1, current.y] == 0)
        {
            neighbors.Add(new Vector2Int(current.x + 1, current.y));
        }

        if (current.y > 0 && grid[current.x, current.y - 1] == 0)
        {
            neighbors.Add(new Vector2Int(current.x, current.y - 1));
        }

        if (current.y < grid.GetLength(1) - 1 && grid[current.x, current.y + 1] == 0)
        {
            neighbors.Add(new Vector2Int(current.x, current.y + 1));
        }

        return neighbors;
    }

    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> parentNodes, Vector2Int current,
        Vector2Int offset)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(new Vector2Int(current.x + (offset.x * width), current.y + (offset.y * width)));
        while (parentNodes.ContainsKey(current))
        {
            current = parentNodes[current];
            // Convert to world coordinates:
            Vector2Int convertedToWorldCoordinates =
                new Vector2Int(current.x + (offset.x * width), current.y + (offset.y * width));
            path.Insert(0, convertedToWorldCoordinates);
        }

        pathCells = path;
        return path;
    }

    void InitializeGrid()
    {
        grid = new int[width, width];
        int numObstacles = (int)(grid.Length * 0.25f);
        // int numObstacles = 0;
        // Randomly set obstacles in grid
        for (int i = 0; i < numObstacles; i++)
        {
            int x = Random.Range(0, grid.GetLength(0));
            int y = Random.Range(0, grid.GetLength(1));
            grid[x, y] = 1;
        }
    }

    private Direction DetermineDirection(Vector2Int start, Vector2Int end)
    {
        Vector2Int startPos = ConvertToGridCoordinates(start);
        Vector2Int endPos = ConvertToGridCoordinates(end);
        if (startPos.x == 0 && endPos.x == 8)
        {
            return Direction.Right;
        }

        if (startPos.y == 0 && endPos.y == 8)
        {
            return Direction.Up;
        }

        if (startPos.x == 8 && endPos.x == 0)
        {
            return Direction.Left;
        }

        if (startPos.x == 0 && endPos.y == 8)
        {
            return Direction.RightUp;
        }

        if (startPos.x == 8 && endPos.y == 8)
        {
            return Direction.LeftUp;
        }

        if (startPos.y == 0 && endPos.x == 0)
        {
            return Direction.UpLeft;
        }

        if (startPos.y == 0 && endPos.x == 8)
        {
            return Direction.UpRight;
        }

        // Not good idea.
        return Direction.Up;
    }

    private Vector2Int ConvertToGridCoordinates(Vector2Int coordinates)
    {
        return new Vector2Int(Mathf.Abs(coordinates.x) % 9, Mathf.Abs(coordinates.y) % 9);
    }

    private int DetermineNeighbourValue(Direction direction, bool isStart, int x, int y,
        PathCellObject[] pathCellObjects)
    {
        NeighbourValueGenerator neighbourValueGenerator = new NeighbourValueGenerator(pathCells);
        switch (direction)
        {
            case Direction.Up:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[0], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[0], x, y);
            case Direction.Right:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[1], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[1], x, y);
            case Direction.Left:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[2], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[2], x, y);
            case Direction.LeftUp:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[3], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[3], x, y);
            case Direction.RightUp:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[4], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[4], x, y);
            case Direction.UpLeft:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[5], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[5], x, y);
            case Direction.UpRight:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[6], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[6], x, y);
            default:
                Debug.Log("Yet not implemented");
                break;
        }

        return 0;
    }
}