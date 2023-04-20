using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private PathGenerator pathGenerator;

    public int gridWidth = 16;
    public int gridHeight = 8;
    public int minPathLength = 25;
    public int blocksSize = 2;

    public GridCellObject[] pathCellsOjbects;
    public GridCellObject[] sceneryCellObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateMultipleGridBlocks());
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
            while (pathSize < minPathLength || !pathCells.Contains(new Vector2Int(endCellX,4)))
            {
                pathCells = pathGenerator.GeneratePath(startingPos);
                pathSize = pathCells.Count;
            }
            yield return StartCoroutine(CreateGrid(pathCells, startingPos, temporaryGridWidth));
            startingPos += 9;
            endCellX = startingPos + 8;
            temporaryGridWidth += gridWidth;
        }
    }

    private IEnumerator CreateGrid(List<Vector2Int> pathCells, int endCellX, int temporaryGridWidth)
    {
        yield return StartCoroutine(LayPathCells(pathCells));
        yield return StartCoroutine(LaySceneryCells(endCellX, temporaryGridWidth));
    }
    
    private IEnumerator LayPathCells(List<Vector2Int> pathCells)
    {
        foreach (var pathCell in pathCells)
        {
            int neigbourValue = pathGenerator.GetCellNeighbourValue(pathCell.x, pathCell.y);
            Debug.Log("Tile " + pathCell.x + "," + pathCell.y + " NValue" +neigbourValue);
            GameObject pathTile = pathCellsOjbects[neigbourValue].cellPrefab;
            GameObject pathTileCell = Instantiate(pathTile, new Vector3(pathCell.x, 0f, pathCell.y), Quaternion.identity);
            pathTileCell.transform.Rotate(0f, pathCellsOjbects[neigbourValue].yRotation, 0f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    private IEnumerator LaySceneryCells(int startingX, int temporaryWidth)
    {
        for (int x = startingX; x < temporaryWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (pathGenerator.CellIsFree(x, y))
                {
                    int randomSceneryCellIndex = Random.Range(0, sceneryCellObjects.Length);
                    Instantiate(sceneryCellObjects[randomSceneryCellIndex].cellPrefab, new Vector3(x, 0f, y), Quaternion.identity);
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        yield return null;
    }
}
