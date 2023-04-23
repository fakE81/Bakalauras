using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathGenerator
{
    public enum Direction {Up, Left, Right, UpLeft, UpRight, LeftUp, RightUp};
    private int[,] grid = new int[9, 9] {
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

    public List<Vector2Int> GeneratePath(int startPosition)
    {
        pathCells = new List<Vector2Int>();

        int y = (int)(height / 2);
        int x = startPosition;
        
        while (x < width)
        {
            pathCells.Add(new Vector2Int(x, y));

            bool validMove = false;
            while (!validMove)
            {
                int move = Random.Range(0, 3);
                if (move == 0 || x % 2 == 0 || x > (width - 2))
                {
                    x++;
                    validMove = true;
                }
                else if (move == 1 && CellIsFree(x,y + 1) && y < (height - 2))
                {
                    y++;
                    validMove = true;
                }
                else if (move == 2 && CellIsFree(x,y - 1) && y > 2)
                {
                    y--;
                    validMove = true;
                }
            }
        }
        
        
        return pathCells;
    }

    public List<Vector2Int> GeneratePath(Vector2Int start, Vector2Int end, Vector2Int offset)
    {
        // Convert to grid coordinates:
        start = ConvertToGridCoordinates(start);
        end = ConvertToGridCoordinates(end);

        InitializeGrid();
        // Create lists for open and closed nodes
        List<Vector2Int> openNodes = new List<Vector2Int>();
        List<Vector2Int> closedNodes = new List<Vector2Int>();

        // Add start node to open list
        openNodes.Add(start);

        // Create dictionary for storing parent nodes
        Dictionary<Vector2Int, Vector2Int> parentNodes = new Dictionary<Vector2Int, Vector2Int>();

        // Create dictionary for storing g scores (cost to move from start to current node)
        Dictionary<Vector2Int, int> gScores = new Dictionary<Vector2Int, int>();
        gScores[start] = 0;

        // Create dictionary for storing f scores (estimated total cost to move from start to end via current node)
        Dictionary<Vector2Int, int> fScores = new Dictionary<Vector2Int, int>();
        fScores[start] = Heuristic(start, end);

        // Loop until open list is empty
        while (openNodes.Count > 0) {
            // Get node with lowest f score
            Vector2Int current = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++) {
                if (fScores.ContainsKey(openNodes[i]) && fScores[openNodes[i]] < fScores[current]) {
                    current = openNodes[i];
                }
            }

            // Check if current node is the end node
            if (current == end) {
                return ReconstructPath(parentNodes, current, offset);
            }

            // Remove current node from open list and add it to closed list
            openNodes.Remove(current);
            closedNodes.Add(current);
            foreach (Vector2Int neighbor in GetNeighbors(current)) {
                // Check if neighbor is already in closed list
                if (closedNodes.Contains(neighbor)) {
                    continue;
                }

                // Calculate tentative g score for neighbor
                int tentativeGScore = gScores[current] + 1;

                // Check if neighbor is not in open list or if tentative g score is lower than existing g score for neighbor
                if (!openNodes.Contains(neighbor) || tentativeGScore < gScores[neighbor]) {
                    // Set parent node for neighbor
                    parentNodes[neighbor] = current;

                    // Set g score for neighbor
                    gScores[neighbor] = tentativeGScore;

                    // Set f score for neighbor
                    fScores[neighbor] = tentativeGScore + Heuristic(neighbor, end);

                    // Add neighbor to open list
                    if (!openNodes.Contains(neighbor)) {
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

    public int GetCellNeighbourValue(int x, int y, Vector2Int startPos, Vector2Int endPos, PathCellObject[] pathCellObjects)
    {
        int returnValue = 0;
        Direction direction = DetermineDirection(startPos, endPos);
        if (startPos.Equals(new Vector2Int(x, y)))
        {
            return DetermineNeighbourValue(direction,true, x, y, pathCellObjects);
        }

        if (endPos.Equals(new Vector2Int(x, y)))
        {
            return DetermineNeighbourValue(direction,false, x, y, pathCellObjects);
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
    
    int Heuristic(Vector2Int current, Vector2Int end) {
        // Manhattan distance heuristic
        return Mathf.Abs(current.x - end.x) + Mathf.Abs(current.y - end.y);
    }

    List<Vector2Int> GetNeighbors(Vector2Int current) {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Check for valid neighbors in grid
        if (current.x > 0 && grid[current.x - 1, current.y] == 0)
        {
            neighbors.Add(new Vector2Int(current.x - 1, current.y));
        }
        if (current.x < grid.GetLength(0) - 1 && grid[current.x + 1, current.y] == 0) {
            neighbors.Add(new Vector2Int(current.x + 1, current.y));
        }
        if (current.y > 0 && grid[current.x, current.y - 1] == 0) {
            neighbors.Add(new Vector2Int(current.x, current.y - 1));
        }
        if (current.y < grid.GetLength(1) - 1 && grid[current.x, current.y + 1] == 0) {
            neighbors.Add(new Vector2Int(current.x, current.y + 1));
        }

        // Shuffle list of neighbors
        for (int i = 0; i < neighbors.Count; i++) {
            int randomIndex = Random.Range(i, neighbors.Count);
            Vector2Int temp = neighbors[i];
            neighbors[i] = neighbors[randomIndex];
            neighbors[randomIndex] = temp;
        }

        return neighbors;
    }

    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> parentNodes, Vector2Int current, Vector2Int offset) {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(new Vector2Int(current.x + (offset.x * width), current.y + (offset.y * width)));
        while (parentNodes.ContainsKey(current)) {
            current = parentNodes[current];
            // Convert to world coordinates:
            Vector2Int convertedToWorldCoordinates =
                new Vector2Int(current.x + (offset.x * width), current.y + (offset.y * width));
            path.Insert(0, convertedToWorldCoordinates);
        }

        pathCells = path;
        return path;
    }
    
    void InitializeGrid() {
        grid = new int[9, 9];
        int numObstacles = (int)(grid.Length * 0.7f); 

        // Randomly set obstacles in grid
        for (int i = 0; i < numObstacles; i++) {
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
        return new Vector2Int(Mathf.Abs(coordinates.x) % 9,Mathf.Abs(coordinates.y) % 9);
    }

    private int DetermineNeighbourValue(Direction direction, bool isStart, int x, int y, PathCellObject[] pathCellObjects)
    {
        NeighbourValueGenerator neighbourValueGenerator = new NeighbourValueGenerator(pathCells);
        switch (direction)
        {
            case Direction.Up:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[0], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[0], x ,y);
            case Direction.Right:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[1], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[1], x ,y);
            case Direction.Left:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[2], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[2], x ,y);
            case Direction.LeftUp:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[3], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[3], x ,y);
            case Direction.RightUp:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[4], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[4], x ,y);
            case Direction.UpLeft:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[5], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[5], x ,y);
            case Direction.UpRight:
                if (isStart)
                    return neighbourValueGenerator.DetermineStartValue(pathCellObjects[6], x, y);
                return neighbourValueGenerator.DetermineEndValue(pathCellObjects[6], x ,y);
            default:
                Debug.Log("Yet not implemented");
                break;
        }

        return 0;
    }
}
