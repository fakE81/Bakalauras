using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContainer : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Vector2Int> pathCells;

    public void UnlockTiles()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        GameObject.FindWithTag("GameController").GetComponent<GridManager>().ShowUnlockButton();
        // TODO: Send pathCell coordinates to waypoint manager for enemies
    }
}
