using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool GameIsOver;
    public GameObject gameOverUI;
    public GameObject pauseUI;
    public GameObject powerUpUI;
    public GameObject completedUI;
    private bool isPaused;

    private Gamestate gamestate;

    public static GameManager instance;

    public GameObject informationUI;

    public static bool DEBUG_MODE = false;

    // Start is called before the first frame update
    void Awake()
    {
        gamestate = Gamestate.Start;
        isPaused = false;
        GameIsOver = false;
        gameOverUI.SetActive(false);
        Time.timeScale = 1;
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameIsOver)
            return;

        if (gamestate == Gamestate.Completed)
        {
            // Show completed UI.
            PlayerStatisticsManager.instance.BanditsHighscore = PlayerStats.Wave;
            completedUI.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                pauseUI.SetActive(true);
                Time.timeScale = 0;
                isPaused = true;
            }
            else
            {
                pauseUI.SetActive(false);
                isPaused = false;
                Time.timeScale = 1;
            }
        }


        if (PlayerStats.CastleHealth <= 0)
        {
            End();
        }
    }

    private void End()
    {
        PlayerStatisticsManager.instance.addCoins(PlayerStats.EARNED_COINS);
        if (PlayerStats.Wave > PlayerStatisticsManager.instance.BanditsHighscore)
        {
            PlayerStatisticsManager.instance.BanditsHighscore = PlayerStats.Wave;
        }
        GameIsOver = true;
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void changeGamestate(Gamestate gamestate)
    {
        this.gamestate = gamestate;
    }

    public Gamestate getGamestate()
    {
        return gamestate;
    }

    public void handleInformationUI(Transform pos, TowerInformation towerInformation)
    {
        if (informationUI.transform.position.Equals(new Vector3(pos.position.x, 0.25f, pos.position.z)) &&
            informationUI.activeSelf)
        {
            informationUI.SetActive(false);
        }
        else
        {
            InformationUI script = informationUI.GetComponent<InformationUI>();
            script.updateInformation(pos, towerInformation);
            informationUI.SetActive(true);
        }
    }
}