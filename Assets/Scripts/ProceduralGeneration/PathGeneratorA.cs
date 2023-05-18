
using System;
using System.Collections.Generic;
using UnityEngine;

public class PathGeneratorA
{
    
    List<Vector2Int> openNodes = new List<Vector2Int>();
    List<Vector2Int> closedNodes = new List<Vector2Int>();
    Dictionary<Vector2Int, Vector2Int> parentNodes = new Dictionary<Vector2Int, Vector2Int>();
    Dictionary<Vector2Int, int> gScores = new Dictionary<Vector2Int, int>();
    Dictionary<Vector2Int, int> fScores = new Dictionary<Vector2Int, int>();
    
    private int[,] grid;
    private List<Vector2Int> pathCells;
    private int gridSize;
    private ANode[,] nodes;
    
    public static int nodesChecked = 0;
    
    private int[] dx = { -1, 1, 0, 0 };
    private int[] dy = { 0, 0, -1, 1 };
    List<ANode> openList = new List<ANode>();
    HashSet<ANode> closedSet = new HashSet<ANode>();
    
    public PathGeneratorA(int [,] grid)
    {
        this.grid = grid;
        gridSize = grid.GetLength(0);
        pathCells = new List<Vector2Int>();
        nodes = new ANode[gridSize, gridSize];
    }

    public List<Vector2Int> GeneratePathNew(Vector2Int start, Vector2Int end)
    {
        ANode startNode = GetNode(start.x, start.y);
        ANode endNode = GetNode(end.x, end.y);

        openList.Add(startNode);
        startNode.G = 0;
        startNode.H = HeuristicCost(startNode, endNode);
        while (openList.Count > 0)
        {
            ANode currentNode = GetLowestFCostNode(openList);

            if (currentNode.X == endNode.X && currentNode.Y == endNode.Y)
            {
                // Found the path
                return ConstructPath(currentNode);
            }

            openList.Remove(currentNode);
            closedSet.Add(currentNode);

            for (int i = 0; i < 4; i++)
            {
                int newX = currentNode.X + dx[i];
                int newY = currentNode.Y + dy[i];

                if (IsWithinGrid(newX, newY) && grid[newX, newY] != -1)
                {
                    ANode neighbor = GetNode(newX, newY);
                    
                    if (closedSet.Contains(neighbor))
                        continue;
                    
                    int tentativeGScore = currentNode.G + 1;
                    if (tentativeGScore < neighbor.G)
                    {
                        neighbor.Parent = currentNode;
                        neighbor.G = tentativeGScore;
                        neighbor.H = HeuristicCost(neighbor, endNode);
                        if (!openList.Contains(neighbor))
                            openList.Add(neighbor);
                    }
                }
            }
            //
            //
            // foreach (var neighbor in GetNeighborsNew(currentNode))
            // {
            //     if (closedSet.Contains(neighbor))
            //         continue;
            //
            //     int tentativeGScore = currentNode.G + 1;
            //
            //     if (tentativeGScore < neighbor.G)
            //     {
            //         neighbor.Parent = currentNode;
            //         neighbor.G = tentativeGScore;
            //         neighbor.H = HeuristicCost(neighbor, endNode);
            //         if (!openList.Contains(neighbor))
            //             openList.Add(neighbor);
            //     }
            // }
            nodesChecked++;
        }

        // No path found
        return null;
    }

    private ANode GetNode(int x, int y)
    {
        if (nodes[x, y] == null)
            nodes[x, y] = new ANode(x, y);

        return nodes[x, y];
    }
    
    private ANode GetLowestFCostNode(List<ANode> nodeList)
    {
        ANode lowestCostNode = nodeList[0];
        foreach (var node in nodeList)
        {
            if (node.F < lowestCostNode.F)
                lowestCostNode = node;
        }
        return lowestCostNode;
    }
    
    private List<ANode> GetNeighborsNew(ANode currentNode)
    {
        List<ANode> neighbors = new List<ANode>();

        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int newX = currentNode.X + dx[i];
            int newY = currentNode.Y + dy[i];

            if (IsWithinGrid(newX, newY) && grid[newX, newY] != -1)
            {
                neighbors.Add(GetNode(newX, newY));
            }
        }

        return neighbors;
    }
    
    private bool IsWithinGrid(int x, int y)
    {
        return x >= 0 && x < gridSize && y >= 0 && y < gridSize;
    }
    
    private int HeuristicCost(ANode node, ANode endNode)
    {
        int dx = Math.Abs(endNode.X - node.X);
        int dy = Math.Abs(endNode.Y - node.Y);
        return dx + dy;
        // int dx = Math.Abs(node.X - endNode.X);
        // int dy = Math.Abs(node.Y - endNode.Y);
        // return Math.Max(dx, dy) + (int)(Math.Sqrt(2) - 1) * Math.Min(dx, dy);
        // int dx = Math.Abs(node.X - endNode.X);
        // int dy = Math.Abs(node.Y - endNode.Y);
        // return Math.Max(dx, dy);
    }
    
    private List<Vector2Int> ConstructPath(ANode endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        ANode currentNode = endNode;
        while (currentNode != null)
        {
            path.Add(new Vector2Int(currentNode.X, currentNode.Y));
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
    
    public List<Vector2Int> GeneratePath(Vector2Int start, Vector2Int end)
    {
        openNodes.Add(start);
        gScores[start] = 0;
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
                return ReconstructPath(current);
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

        return neighbors;
    }

    List<Vector2Int> ReconstructPath(Vector2Int current) {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(new Vector2Int(current.x, current.y));
        while (parentNodes.ContainsKey(current)) {
            current = parentNodes[current];
            // Convert to world coordinates:
            Vector2Int convertedToWorldCoordinates =
                new Vector2Int(current.x, current.y);
            path.Insert(0, convertedToWorldCoordinates);
        }

        pathCells = path;
        return path;
    }
}
