using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    private PathGenerator pathGenerator;

    public bool isDemo = false;

    public int gridWidth = 16;
    public int gridHeight = 8;
    public int minPathLength = 25;
    public int blocksSize = 2;
    public int pathDirectionRestriction;
    public Vector2Int startPos;
    public Vector2Int endPos;
    private int pathDirection = 0;
    private Vector2Int pathOffset = new Vector2Int(10, 0);
    private Vector2Int sceneryLayOffset = new Vector2Int(9 * 10, 0);

    public GridCellObject[] pathCellsOjbects;
    public GridCellObject[] sceneryCellObjects;
    public PathCellObject[] startEndPathObjects;

    public GameObject tileContainer;
    private LinkedList<GameObject> tileContainerList = new LinkedList<GameObject>();

    private bool unlockNextTile;

    // Start is called before the first frame update
    void Start()
    {
        unlockNextTile = false;
        StartCoroutine(CreateMultipleDEMO());
    }

    private void Update()
    {
        if (unlockNextTile && !isDemo)
        {
            ShowUnlockButton();
            unlockNextTile = false;
        }
    }

    private IEnumerator CreateMultipleDEMO()
    {
        // First Tyle type:
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

            yield return StartCoroutine(CreateGrid(pathCells, sceneryLayOffset));
            currentType = DetermineNextTileType(currentType);
            DetermineOffsets(currentType);
            if (isDemo)
            {
                Debug.Log("Generated path in " + timesGenerated + " times!!");
                Debug.Log("Start coordinates: " + startPos.x + " " + startPos.y);
                Debug.Log("End coordinates: " + endPos.x + " " + endPos.y);
            }
        }

        unlockNextTile = true;
    }

    private IEnumerator CreateGrid(List<Vector2Int> pathCells, Vector2Int sceneryLayOffset)
    {
        // Parent Gameobject:
        GameObject tileHolder = Instantiate(tileContainer, CalculateMiddlePoint(), Quaternion.identity);
        yield return StartCoroutine(LayPathCells(pathCells, tileHolder));
        yield return StartCoroutine(LaySceneryCells(sceneryLayOffset, tileHolder));
        if (!isDemo)
        {
            tileHolder.transform.GetChild(0).gameObject.SetActive(false);
            tileHolder.GetComponent<TileContainer>().pathCells = pathCells;
            tileContainerList.AddFirst(tileHolder);
        }
    }

    private IEnumerator LayPathCells(List<Vector2Int> pathCells, GameObject tileHolder)
    {
        foreach (var pathCell in pathCells)
        {
            int neigbourValue =
                pathGenerator.GetCellNeighbourValue(pathCell.x, pathCell.y, startPos, endPos, startEndPathObjects);
            Debug.Log("Tile " + pathCell.x + "," + pathCell.y + " NValue" + neigbourValue);
            GameObject pathTile = pathCellsOjbects[neigbourValue].cellPrefab;
            GameObject pathTileCell =
                Instantiate(pathTile, new Vector3(pathCell.x, 0f, pathCell.y), Quaternion.identity);
            pathTileCell.transform.Rotate(0f, pathCellsOjbects[neigbourValue].yRotation, 0f);
            pathTileCell.transform.SetParent(tileHolder.transform.GetChild(0).transform);
            yield return new WaitForSeconds(0.03f);
        }

        yield return null;
    }

    private IEnumerator LaySceneryCells(Vector2Int sceneryLayOffset, GameObject tileHolder)
    {
        for (int x = sceneryLayOffset.x; x < sceneryLayOffset.x + gridWidth; x++)
        {
            for (int y = sceneryLayOffset.y; y < sceneryLayOffset.y + gridHeight; y++)
            {
                if (pathGenerator.CellIsFree(x, y))
                {
                    int randomSceneryCellIndex = Random.Range(0, sceneryCellObjects.Length);
                    var sceneryObject = Instantiate(sceneryCellObjects[randomSceneryCellIndex].cellPrefab,
                        new Vector3(x, 0f, y),
                        Quaternion.identity);
                    sceneryObject.transform.SetParent(tileHolder.transform.GetChild(0).transform);
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }

        yield return null;
    }

    private TileType DetermineNextTileType(TileType type)
    {
        if (pathDirection <= -pathDirectionRestriction && type == TileType.LeftLeft)
        {
            return TileType.LeftUp;
        }

        if (pathDirection <= -pathDirectionRestriction && type == TileType.LeftUp)
        {
            return TileType.UpRight;
        }

        if (pathDirection >= pathDirectionRestriction && type == TileType.RightRight)
        {
            return TileType.RightUp;
        }

        if (pathDirection >= pathDirectionRestriction && type == TileType.RightUp)
        {
            return TileType.UpLeft;
        }

        if (type == TileType.LeftLeft || type == TileType.UpLeft)
        {
            TileType[] types = { TileType.LeftLeft, TileType.LeftUp };
            return types[Random.Range(0, 2)];
        }

        if (type == TileType.LeftUp || type == TileType.RightUp || type == TileType.UpUp)
        {
            TileType[] types = { TileType.UpLeft, TileType.UpRight, TileType.UpUp };
            return types[Random.Range(0, 3)];
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
                sceneryLayOffset.x -= gridWidth;
                startPos = new Vector2Int(endPos.x - 1, endPos.y);
                endPos = new Vector2Int(endPos.x - gridWidth, endPos.y);
                pathDirection--;
                break;
            case TileType.LeftUp:
                pathOffset.x--;
                sceneryLayOffset.x -= gridWidth;
                startPos = new Vector2Int(endPos.x - 1, endPos.y);
                endPos = new Vector2Int(endPos.x - 5, endPos.y + 4);
                pathDirection--;
                break;
            case TileType.RightRight:
                pathOffset.x++;
                sceneryLayOffset.x += gridWidth;
                startPos = new Vector2Int(endPos.x + 1, endPos.y);
                endPos = new Vector2Int(endPos.x + gridWidth, endPos.y);
                pathDirection++;
                break;
            case TileType.RightUp:
                pathOffset.x++;
                sceneryLayOffset.x += gridWidth;
                startPos = new Vector2Int(endPos.x + 1, endPos.y);
                endPos = new Vector2Int(endPos.x + 5, endPos.y + 4);
                pathDirection++;
                break;
            case TileType.UpUp:
                pathOffset.y++;
                sceneryLayOffset.y += gridWidth;
                startPos = new Vector2Int(endPos.x, endPos.y + 1);
                endPos = new Vector2Int(endPos.x, endPos.y + gridWidth);
                break;
            case TileType.UpRight:
                pathOffset.y++;
                sceneryLayOffset.y += gridWidth;
                startPos = new Vector2Int(endPos.x, endPos.y + 1);
                endPos = new Vector2Int(endPos.x + 4, endPos.y + 5);
                break;
            case TileType.UpLeft:
                pathOffset.y++;
                sceneryLayOffset.y += gridWidth;
                startPos = new Vector2Int(endPos.x, endPos.y + 1);
                endPos = new Vector2Int(endPos.x - 4, endPos.y + 5);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private Vector3 CalculateMiddlePoint()
    {
        return new Vector3(sceneryLayOffset.x + 4, 0, sceneryLayOffset.y + 4);
    }

    public void ShowUnlockButton()
    {
        if (tileContainerList.Count != 0)
        {
            tileContainerList.Last().transform.GetChild(1).gameObject.SetActive(true);
            tileContainerList.RemoveLast();   
        }
        else
        {
            Debug.Log("Tile container is empty!");
        }
    }
    
}