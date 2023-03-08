using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static int enemiesCount;


    public Wave[] wave;
    private Wave currentWave;
    public Transform enemyPrefab;

    [SerializeField] private Transform spawnPoint;

    private int waveIndex = 1;
    private float multiplier = 1;
    private GameManager gameManager;
    [Header("DEBUG")] public bool turnOffEnemies = true;

    void Start()
    {
        gameManager = GameManager.instance;
        currentWave = wave[0];
        enemiesCount = 0;
    }

    void Update()
    {
        if (turnOffEnemies)
            return;

        // Laukiam ir tada paleidziam.
        if (Gamestate.StartSpawningWave == gameManager.getGamestate())
        {
            // Get wave:
            ChangeBaseWave();
            spawnPoint = Waypoints.getStartWaypoint();
            PlayerStats.Wave = waveIndex;
            //Spawn enemies
            for (int i = 0; i < currentWave.enemies.Length; i++)
            {
                StartCoroutine(SpawnWave(currentWave.enemies[i], currentWave.enemies[i].rateMin,
                    currentWave.enemies[i].rateMax));
            }

            gameManager.changeGamestate(Gamestate.StopSpawningWave);
        }
        else if (gameManager.getGamestate() == Gamestate.EnemiesSpawned && enemiesCount == 0)
        {
            gameManager.changeGamestate(Gamestate.ClearedWave);
            gameManager.selectPowerUI();
            multiplier += 0.3f;
            waveIndex++;
        }
    }

    IEnumerator SpawnWave(EnemiesInformation enemy, float rateMin, float rateMax)
    {
        //waveIndex++;
        int enemiesToSpawn = Mathf.RoundToInt(enemy.count * multiplier);
        enemiesCount += enemiesToSpawn;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy(enemy.enemy);
            yield return new WaitForSeconds(Random.Range(rateMin, rateMax));
        }

        gameManager.changeGamestate(Gamestate.EnemiesSpawned);
    }

    void SpawnEnemy(GameObject enemy)
    {
        //enemiesCount++;
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
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