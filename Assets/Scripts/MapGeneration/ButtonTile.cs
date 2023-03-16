using UnityEngine;
using UnityEngine.UI;

public class ButtonTile : MonoBehaviour
{
    public Button expandButton;
    public TileType[] possibleTiles;
    [SerializeField] private TileGeneration tileGeneration;

    public TileType[] PossibleTiles
    {
        get => possibleTiles;
        set => possibleTiles = value;
    }

    public void Start()
    {
        tileGeneration = GameObject.FindGameObjectWithTag("MapGeneration").GetComponent<TileGeneration>();
        if (GameManager.instance.getGamestate() != Gamestate.Start)
        {
            expandButton.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (GameManager.instance.getGamestate() == Gamestate.ClearedWave && !expandButton.gameObject.activeSelf)
        {
            expandButton.gameObject.SetActive(true);
        }
    }

    public void Unlock()
    {
        //Button pressed to unlock.
        tileGeneration.GenerateTile(possibleTiles, this.transform);
        Destroy(gameObject);
    }
}