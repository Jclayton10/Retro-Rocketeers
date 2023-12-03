using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    [SerializeField] private float countdown;
    public GameObject[] spawnPoints; // Array to store spawn points

    public Wave[] waves;
    [HideInInspector] public int currentWaveIndex = 0;

    private bool readyToCountDown;

    void Start()
    {
        readyToCountDown = true;
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }

    void Update()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("You survived every wave!");
            return;
        }

        if (readyToCountDown)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0)
        {
            readyToCountDown = false;
            countdown = waves[currentWaveIndex].timeToNextWave;
            StartCoroutine(SpawnWave());
        }

        if (waves[currentWaveIndex].enemiesLeft == 0)
        {
            readyToCountDown = true;
            currentWaveIndex++;
        }
    }

    private IEnumerator SpawnWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
            {
                // Get a random spawn point from the array
                GameObject randomSpawnPoint = GetRandomSpawnPoint();

                // Instantiate the enemy at the random spawn point's position
                Instantiate(waves[currentWaveIndex].enemies[i], randomSpawnPoint.transform.position, Quaternion.identity);

                yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
            }
        }
    }

    private GameObject GetRandomSpawnPoint()
    {
        // Choose a random index from the spawnPoints array
        int randomIndex = Random.Range(0, spawnPoints.Length);

        // Return the GameObject at the random index
        return spawnPoints[randomIndex];
    }
}

[System.Serializable]
public class Wave
{
    public GameObject[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}
