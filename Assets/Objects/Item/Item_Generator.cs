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
    GameObject fish;
    void Start()
    {
        timer = 0;
        fish = GameObject.Find("Fish");
    }

    private void Update()
    {
        timer-=Time.deltaTime;
        if(timer < 0)
        {
            timer = spawnInterval;
            SpawnRandomItem();
            SpawnRandomItem();
        }
    }

    void SpawnRandomItem()
    {
        float fishX = fish.transform.position.x;
        float fishY = fish.transform.position.y;
        float randomX = Random.Range(-spawnRange.x+ fishX, spawnRange.x+ fishX);
        float randomY = Random.Range(-spawnRange.y+ fishY, spawnRange.y+ fishY);
        Vector2 spawnPosition = new Vector2(randomX, randomY) + (Vector2)transform.position;

        GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        Instantiate(randomItemPrefab, spawnPosition, Quaternion.identity);
    }
}
