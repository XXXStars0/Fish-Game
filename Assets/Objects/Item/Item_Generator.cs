using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Item_Generator : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Vector2 spawnRange;
    public float spawnInterval = 5f;

    [SerializeField] float timer;
    void Start()
    {
        timer = spawnInterval;
    }

    private void Update()
    {
        timer-=Time.deltaTime;
        if(timer < 0)
        {
            timer = spawnInterval;
            SpawnRandomItem();
        }
    }

    void SpawnRandomItem()
    {
        float randomX = Random.Range(-spawnRange.x, spawnRange.x);
        float randomY = Random.Range(-spawnRange.y, spawnRange.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY) + (Vector2)transform.position;

        GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        Instantiate(randomItemPrefab, spawnPosition, Quaternion.identity);
    }
}
