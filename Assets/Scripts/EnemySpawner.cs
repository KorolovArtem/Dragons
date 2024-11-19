using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int maxEnemies = 2;
    [SerializeField] private float increaseInterval = 30f;
    [SerializeField] private int increaseAmount = 2;

    private float spawnTimer;
    private float timeSinceStart;
    private bool isSpawning = true;

    void Start()
    {
        spawnTimer = spawnInterval;
        timeSinceStart = 0f;
    }

    void Update()
    {
        if (!isSpawning) return;

        timeSinceStart += Time.deltaTime;
        if (timeSinceStart >= increaseInterval)
        {
            maxEnemies += increaseAmount;
            timeSinceStart -= increaseInterval;
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            if (GetActiveEnemiesCount() < maxEnemies)
            {
                SpawnEnemy();
            }
            spawnTimer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject prefab = enemyPrefabs[enemyIndex];

        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }

    int GetActiveEnemiesCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void ResumeSpawning()
    {
        isSpawning = true;
    }

    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
    }

    public void SetIncreaseInterval(float interval)
    {
        increaseInterval = interval;
    }

    public void SetIncreaseAmount(int amount)
    {
        increaseAmount = amount;
    }
}