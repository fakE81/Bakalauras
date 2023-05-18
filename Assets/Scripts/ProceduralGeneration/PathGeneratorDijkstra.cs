

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PathGeneratorDijkstra
{
    
    private const int Infinity = int.MaxValue;

    public int[,] grid;
    private int rows;
    private int columns;
    private int size;

    public static int nodesChecked = 0;

    private Vector2Int[] delta = {
        new Vector2Int(-1, 0), // Up
        new Vector2Int(0, 1), // Right
        new Vector2Int(1, 0), // Down
        new Vector2Int(0, -1) // Left
    };

    private int[,] shortestDistances;
    private Vector2Int[,] previousCells;

    private List<Vector2Int> pathCells;

    public PathGeneratorDijkstra(int size)
    {
        this.size = size;
    }

    public List<Vector2Int> ComputeShortestDistances(Vector2Int start, Vector2Int end, int[,] grid)
    {
        this.grid = grid;
        return ComputeShortestDistances(start, end, false);
    }
    
    public List<Vector2Int> ComputeShortestDistances(Vector2Int start, Vector2Int end, bool generateGrid)
    {
        if (generateGrid)
        {
            grid = GenerateGrid(size, size);
        }
        rows = grid.GetLength(0);
        columns = grid.GetLength(1);
        shortestDistances = new int[rows, columns];
        previousCells = new Vector2Int[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                shortestDistances[row, col] = Infinity;
                previousCells[row, col] = Vector2Int.one * -1;
            }
        }

        shortestDistances[start.x, start.y] = 0;
        var queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var currentCell = queue.Dequeue();

            for (int i = 0; i < delta.Length; i++)
            {
                var newCell = currentCell + delta[i];

                if (IsValidCell(newCell) && grid[newCell.x, newCell.y] != -1) // Check for obstacles
                {
                    int newDistance = shortestDistances[currentCell.x, currentCell.y] + grid[newCell.x, newCell.y];
                    if (newDistance < shortestDistances[newCell.x, newCell.y])
                    {
                        shortestDistances[newCell.x, newCell.y] = newDistance;
                        previousCells[newCell.x, newCell.y] = currentCell;
                        queue.Enqueue(newCell);
                    }
                }
            }

            nodesChecked++;
        }

        return GetShortestPath(end);
    }

    private bool IsValidCell(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < rows && cell.y >= 0 && cell.y < columns;
    }

    private List<Vector2Int> GetShortestPath(Vector2Int end)
    {
        var path = new List<Vector2Int>();
        if (shortestDistances[end.x, end.y] == Infinity)
            return null; // No path found

        var currentCell = end;
        while (currentCell != Vector2Int.one * -1)
        {
            path.Add(currentCell);
            currentCell = previousCells[currentCell.x, currentCell.y];
        }

        path.Reverse();
        // path = PrintShortestPath(path);
        pathCells = path;
        return path;
    }

    public List<Vector2Int> PrintShortestPath(List<Vector2Int> path)
    {
        List<Vector2Int> newPath = new List<Vector2Int>();
        foreach (var vector2Int in path)
        {
            int x = vector2Int.x + 10 * 9;
            newPath.Add(new Vector2Int(x, vector2Int.y));
        }

        return newPath;
    }

    public int[,] GenerateGrid(int rows, int columns)
    {
        int[,] grid = new int[rows, columns];
        Random random = new Random();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Assign random values to grid cells
                grid[row, col] = 1;
                if (random.NextDouble() < 0.2) // Adjust the probability (e.g., 0.2 for 20% chance)
                {
                    grid[row, col] = -1; // Mark cell as an obstacle
                }
            }
        }

        return grid;
    }
}
