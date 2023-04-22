using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    private PathGenerator pathGenerator;

    public int gridWidth = 16;
    public int gridHeight = 8;
    public int minPathLength = 25;
    public int blocksSize = 2;
    public Vector2Int startPos;
    public Vector2Int endPos;
    private Vector2Int pathOffset = new Vector2Int(0,0);
    private Vector2Int sceneryLayOffset = new Vector2Int(0, 0);

    public GridCellObject[] pathCellsOjbects;
    public GridCellObject[] sceneryCellObjects;
    public PathCellObject[] startEndPathObjects;

    // Start is called before the first frame update
    void Start()
    {
        // int startingPos = 0;
        // int endCellX = 8;
        // int temporaryGridWidth = gridWidth;
        // pathGenerator = new PathGenerator(temporaryGridWidth, gridHeight);
        //
        // List<Vector2Int> pathCells = pathGenerator.GeneratePath(startPos, endPos);
        // int count = 0;
        // while (pathCells == null || count < minPathLength)
        // {
        //     pathCells = pathGenerator.GeneratePath(startPos, endPos);
        //     if (pathCells != null)
        //     {
        //         count = pathCells.Count;
        //     }
        // }
        // int pathSize = pathCells.Count;
        // // For galima cia butu ideti
        // while (pathSize < minPathLength)
        // {
        //     pathCells = pathGenerator.GeneratePath(new Vector2Int(0,4), new Vector2Int(8,4));
        //     pathSize = pathCells.Count;
        // }

        StartCoroutine(CreateMultipleDEMO());
    }

    private IEnumerator CreateMultipleDEMO()
    {
        TileType currentType = TileType.UpUp;
        for (int i = 0; i < blocksSize; i++)
        {
            int timesGenerated = 1;
            pathGenerator = new PathGenerator(gridWidth, gridHeight);

            List<Vector2Int> pathCells = pathGenerator.GeneratePath(startPos, endPos, pathOffset);
            int count = 0;
            while (pathCells == null || count < minPathLength)
            {
                pathCells = pathGenerator.GeneratePath(startPos, endPos, pathOffset);
                timesGenerated++;
                if (pathCells != null)
                {
                    count = pathCells.Count;
                }
            }
            Debug.Log("Generated path in " + timesGenerated + " times!!");

            yield return StartCoroutine(CreateGrid(pathCells, sceneryLayOffset));
            // Pagal direction turetu keistis.
            currentType = DetermineNextTileType(currentType);
            Debug.Log("Next Tile type:" + currentType.ToString());
            DetermineOffsets(currentType);
            Debug.Log("Start coordinates: " + startPos.x + " " + startPos.y);
            Debug.Log("End coordinates: " + endPos.x + " " + endPos.y);
            // startPos = new Vector2Int(endPos.x + 1, endPos.y);
            // endPos = new Vector2Int(endPos.x + gridWidth, endPos.y);
            // offset.x++;
            // sceneryLayOffset.x+=gridWidth; // Depends kur stumames
        }
    }

    private IEnumerator CreateMultipleGridBlocks()
    {
        int startingPos = 0;
        int endCellX = 8;
        int temporaryGridWidth = gridWidth;
        for (int i = 0; i < blocksSize; i++)
        {
            pathGenerator = new PathGenerator(temporaryGridWidth, gridHeight);

            List<Vector2Int> pathCells = pathGenerator.GeneratePath(startingPos);
            int pathSize = pathCells.Count;
            // For galima cia butu ideti
            while (pathSize < minPathLength || !pathCells.Contains(new Vector2Int(endCellX, 4)))
            {
                pathCells = pathGenerator.GeneratePath(startingPos);
                pathSize = pathCells.Count;
            }

            yield return StartCoroutine(CreateGrid(pathCells, new Vector2Int()));
            startingPos += 9;
            endCellX = startingPos + 8;
            temporaryGridWidth += gridWidth;
        }
    }

    private IEnumerator CreateGrid(List<Vector2Int> pathCells, Vector2Int sceneryLayOffset)
    {
        yield return StartCoroutine(LayPathCells(pathCells));
        yield return StartCoroutine(LaySceneryCells(sceneryLayOffset));
    }

    private IEnumerator LayPathCells(List<Vector2Int> pathCells)
    {
        foreach (var pathCell in pathCells)
        {
            int neigbourValue = pathGenerator.GetCellNeighbourValue(pathCell.x, pathCell.y, startPos, endPos, startEndPathObjects);
            //Debug.Log("Tile " + pathCell.x + "," + pathCell.y + " NValue" + neigbourValue);
            GameObject pathTile = pathCellsOjbects[neigbourValue].cellPrefab;
            GameObject pathTileCell =
                Instantiate(pathTile, new Vector3(pathCell.x, 0f, pathCell.y), Quaternion.identity);
            pathTileCell.transform.Rotate(0f, pathCellsOjbects[neigbourValue].yRotation, 0f);
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    private IEnumerator LaySceneryCells(Vector2Int sceneryLayOffset)
    {
        for (int x = sceneryLayOffset.x; x < sceneryLayOffset.x+gridWidth; x++)
        {
            for (int y = sceneryLayOffset.y; y < sceneryLayOffset.y + gridHeight; y++)
            {
                if (pathGenerator.CellIsFree(x, y))
                {
                    int randomSceneryCellIndex = Random.Range(0, sceneryCellObjects.Length);
                    Instantiate(sceneryCellObjects[randomSceneryCellIndex].cellPrefab, new Vector3(x, 0f, y),
                        Quaternion.identity);
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }

        yield return null;
    }

    private TileType DetermineNextTileType(TileType type)
    {
        // if (type == TileType.LeftLeft || type == TileType.UpLeft)
        // {
        //     TileType[] types = { TileType.LeftLeft, TileType.LeftUp };
        //     return types[Random.Range(0, 1)];
        // }

        if (type == TileType.LeftUp || type == TileType.RightUp || type == TileType.UpUp)
        {
            TileType[] types = {TileType.UpRight };
            return types[Random.Range(0, 1)];
        }

        if (type == TileType.RightRight || type == TileType.UpRight)
        {
            TileType[] types = { TileType.RightRight, TileType.RightUp };
            return types[Random.Range(0, 2)];
        }
        Debug.LogError("Next Tile type doesn't exist");
        return TileType.LeftLeft;
    }

    private void DetermineOffsets(TileType type)
    {
        switch (type)
        {
            case TileType.LeftLeft:
                pathOffset.x--;
                sceneryLayOffset.x-=gridWidth;
                startPos = new Vector2Int(endPos.x - 1, endPos.y);
                endPos = new Vector2Int(endPos.x - gridWidth, endPos.y);
                break;
            case TileType.LeftUp:
                pathOffset.x--;
                sceneryLayOffset.x-=gridWidth;
                startPos = new Vector2Int(endPos.x - 1, endPos.y);
                endPos = new Vector2Int(endPos.x - 4, endPos.y + 4);
                break;
            case TileType.RightRight:
                pathOffset.x++;
                sceneryLayOffset.x+=gridWidth;
                startPos = new Vector2Int(endPos.x + 1, endPos.y);
                endPos = new Vector2Int(endPos.x + gridWidth, endPos.y);
                break;
            case TileType.RightUp:
                pathOffset.x++;
                sceneryLayOffset.x+=gridWidth;
                startPos = new Vector2Int(endPos.x + 1, endPos.y);
                endPos = new Vector2Int(endPos.x + 5, endPos.y + 4);
                break;
            case TileType.UpUp:
                pathOffset.y++;
                sceneryLayOffset.y+=gridWidth;
                startPos = new Vector2Int(endPos.x, endPos.y + 1);
                endPos = new Vector2Int(endPos.x, endPos.y + gridWidth);
                break;
            case TileType.UpRight:
                pathOffset.y++;
                sceneryLayOffset.y+=gridWidth;
                startPos = new Vector2Int(endPos.x, endPos.y + 1);
                endPos = new Vector2Int(endPos.x + 4, endPos.y + 5);
                break;
            case TileType.UpLeft:
                pathOffset.y++;
                sceneryLayOffset.y+=gridWidth;
                startPos = new Vector2Int(endPos.x, endPos.y + 1);
                endPos = new Vector2Int(endPos.x - 4, endPos.y + 5);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}