using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool GameIsOver;
    public GameObject gameOverUI;
    public GameObject pauseUI;
    public GameObject powerUpUI;
    private bool isPaused;

    private Gamestate gamestate;

    public static GameManager instance;
    private PowerCardsHandler powerCardsHandler;

    public GameObject informationUI;

    public static bool DEBUG_MODE = false;

    // Start is called before the first frame update
    void Awake()
    {
        powerCardsHandler = gameObject.GetComponent<PowerCardsHandler>();
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
        ExperienceManager.instance.HandleLevelUps();
        GameIsOver = true;
        gameOverUI.SetActive(true);
        // Sustabdom zaidima.
        Time.timeScale = 0;
    }

    public void SelectedPower(string msg)
    {
        Debug.Log(msg);
        // Send that message to powerup class:
        powerCardsHandler.handle(msg);
        powerUpUI.SetActive(false);
    }

    public void selectPowerUI()
    {
        powerUpUI.SetActive(true);
    }

    public void changeGamestate(Gamestate gamestate)
    {
        this.gamestate = gamestate;
    }

    public Gamestate getGamestate()
    {
        return gamestate;
    }

    public void handleInformationUI(Transform pos, float damage, float range, float fireRate, float turnSpeed)
    {
        if (informationUI.transform.position.Equals(new Vector3(pos.position.x, 3.4f, pos.position.z)) &&
            informationUI.activeSelf)
        {
            informationUI.SetActive(false);
        }
        else
        {
            InformationUI script = informationUI.GetComponent<InformationUI>();
            script.updateInformation(pos, damage, range, fireRate, turnSpeed);
            informationUI.SetActive(true);
        }
    }
}