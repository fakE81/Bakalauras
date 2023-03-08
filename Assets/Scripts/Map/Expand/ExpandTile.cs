using UnityEngine;
using UnityEngine.UI;

public class ExpandTile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] tiles;
    private ExpandTileGenerator expandTileGenerator;
    private GameManager gameManager;
    public Button expandButton;


    void Start()
    {
        expandTileGenerator = GameObject.FindGameObjectWithTag("MapGeneration").GetComponent<ExpandTileGenerator>();
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
//        Debug.Log(gameManager.getGamestate());
        if (gameManager.getGamestate() == Gamestate.ClearedWave || gameManager.getGamestate() == Gamestate.Start ||
            GameManager.DEBUG_MODE)
        {
            expandButton.interactable = true;
            return;
        }

        expandButton.interactable = false;
    }

    public void expandOnClick()
    {
        int[] possibleTilesIndexes = possibleList();
        if (possibleTilesIndexes == null)
        {
            Debug.LogError("PossibleTilesIndexes is null!!");
        }

        int index = Random.Range(0, possibleTilesIndexes.Length);
        GameObject tile = tiles[possibleTilesIndexes[index]];
        GameObject tileObject = Instantiate(tile,
            new Vector3(this.gameObject.transform.position.x, 0f, this.gameObject.transform.position.z),
            tile.gameObject.transform.rotation);
        UnlockedTilesRepository.addTile(tileObject);
        // Send message that I need more expand buttons!!!
        expandTileGenerator.addExpandButtons(tile.GetComponent<UnlockedTile>().getID(), transform.position);

        ExpandTileRepository.removeGameobject(this.gameObject);

        Destroy(this.gameObject);
    }

    public int[] possibleList()
    {
        // I kaire:
        if (UnlockedTilesRepository.findDownTiles(gameObject.transform.position.x, gameObject.transform.position.z)
                .Count >= 0 && UnlockedTilesRepository
                .findRightTiles(gameObject.transform.position.x, gameObject.transform.position.z).Count == 1)
        {
            int[] possibleTiles = { 0, 4 };
            return possibleTiles;
        }
        // Desine
        else if (UnlockedTilesRepository.findDownTiles(gameObject.transform.position.x, gameObject.transform.position.z)
                     .Count >= 0 && UnlockedTilesRepository
                     .findLeftTiles(gameObject.transform.position.x, gameObject.transform.position.z).Count == 1)
        {
            int[] possibleTiles = { 7, 5 };
            return possibleTiles;
        }
        // I virsu
        else if (UnlockedTilesRepository.findDownTiles(gameObject.transform.position.x, gameObject.transform.position.z)
                     .Count == 1 || (gameObject.transform.position.x == 0 && gameObject.transform.position.z == 10))
        {
            int[] possibleTiles = { 1, 3, 6 };
            return possibleTiles;
        }

        return null;
    }
}