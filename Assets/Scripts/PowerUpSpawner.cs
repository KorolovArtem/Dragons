using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private float spawnX = 10f;
    [SerializeField] private float spawnYMin = -3f;
    [SerializeField] private float spawnYMax = 3f;

    void Start()
    {
        InvokeRepeating("SpawnPowerUp", spawnInterval, spawnInterval);
    }

    void SpawnPowerUp()
    {
        float spawnY = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }
}