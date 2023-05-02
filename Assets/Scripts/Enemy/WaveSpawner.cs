using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static int enemiesCount;


    public Wave[] wave;
    private Wave currentWave;
    public Transform enemyPrefab;

    [SerializeField] private Vector3 spawnPoint;

    private int waveIndex = 1;
    private float multiplier = 1;
    private bool finishedSpawning;
    private bool startedSpawning;
    private GameManager gameManager;
    [Header("DEBUG")] public bool turnOffEnemies = true;

    void Start()
    {
        gameManager = GameManager.instance;
        currentWave = wave[0];
        enemiesCount = 0;
        finishedSpawning = false;
        startedSpawning = false;
    }

    void Update()
    {
        if (turnOffEnemies)
            return;

        // Laukiam ir tada paleidziam.
        if (Gamestate.StartedWave == gameManager.getGamestate() && !startedSpawning)
        {
            // Get wave:
            startedSpawning = true;
            finishedSpawning = false;
            ChangeBaseWave();
            spawnPoint = Waypoints.getStartWaypoint();
            PlayerStats.Wave = waveIndex;
            //Spawn enemies
            for (int i = 0; i < currentWave.enemies.Length; i++)
            {
                StartCoroutine(SpawnWave(currentWave.enemies[i], currentWave.enemies[i].rateMin,
                    currentWave.enemies[i].rateMax, currentWave.times, currentWave.timeRange));
            }
        }
        else if (finishedSpawning && enemiesCount == 0)
        {
            startedSpawning = false;
            finishedSpawning = false;
            GameObject.FindWithTag("GameController").GetComponent<GridManager>().ShowUnlockButton();
            //gameManager.selectPowerUI();
            multiplier += 0.3f;
            waveIndex++;
            if (waveIndex == 11)
            {
                // Map is completed
                gameManager.changeGamestate(Gamestate.Completed);
            }
            else
            {
                gameManager.changeGamestate(Gamestate.ClearedWave);
            }
        }
    }

    IEnumerator SpawnWave(EnemiesInformation enemy, float rateMin, float rateMax, int times, float timeRange)
    {
        //waveIndex++;
        for (int i = 0; i < times; i++)
        {
            int enemiesToSpawn = Mathf.RoundToInt(enemy.count * multiplier);
            enemiesCount += enemiesToSpawn;
            for (int j = 0; j < enemiesToSpawn; j++)
            {
                yield return new WaitForSeconds(Random.Range(rateMin, rateMax));
                SpawnEnemy(enemy.enemy);
            }
            yield return new WaitForSeconds(timeRange);
        }

        finishedSpawning = true;
    }

    void SpawnEnemy(GameObject enemy)
    {
        //enemiesCount++;
        Instantiate(enemy, spawnPoint, Quaternion.identity);
    }

    private void ChangeBaseWave()
    {
        for (int i = 1; i < wave.Length; i++)
        {
            if (waveIndex == wave[i].startLevel)
            {
                currentWave = wave[i];
                multiplier = 1;
            }
        }
    }
}