using UnityEngine;
using System.Collections.Generic;

public class HeartSpawner : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private float spawnInterval = 15f;
    [SerializeField] private float spawnX = 12f;
    [SerializeField] private float spawnYMin = -2f;
    [SerializeField] private float spawnYMax = 3f;
    [SerializeField] private int poolSize = 5;

    private List<GameObject> heartPool;
    private float nextSpawnTime;

    void Start()
    {
        heartPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject heart = Instantiate(heartPrefab);
            heart.SetActive(false);
            heartPool.Add(heart);
        }

        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnHeart();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnHeart()
    {
        float spawnY = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject heart = GetPooledHeart();
        if (heart != null)
        {
            heart.transform.position = spawnPosition;
            heart.SetActive(true);
        }
    }

    GameObject GetPooledHeart()
    {
        foreach (GameObject heart in heartPool)
        {
            if (heart != null && !heart.activeInHierarchy)
            {
                return heart;
            }
        }
        return null;
    }
}