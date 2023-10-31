using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject player;
    public Transform spawnPoint;

    public float maxSpawnRange;
    public float minSpawnRange;


    public float spwanInterval = 2f;
    public int maxNumberOfEnemies = 10;

    int numberOfEnemies;

    private float nextSpawnTime = 0f;
    // Start is called before the first frame update
    void Start()
    {


        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, spawnPoint.position);


        Debug.Log("Number Of Enemies " + numberOfEnemies);
        Debug.Log("Distance to Player: " + distanceToPlayer);

        if (distanceToPlayer >= minSpawnRange && distanceToPlayer <= maxSpawnRange)
        {
            if (numberOfEnemies <= maxNumberOfEnemies)
            {
                if (Time.time >= nextSpawnTime)
                {
                    SpwanEnemy();
                    numberOfEnemies += 1;
                    nextSpawnTime = Time.time + spwanInterval;

                }
            }
        }


    }
    void SpwanEnemy()
    {

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[randomIndex];

        Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
    }
}


