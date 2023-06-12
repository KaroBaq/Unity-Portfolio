using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject[] powerUpPrefabs;

    private float spawnRange = 9.0f;
    public int enemyCount;
    private int waveNumber = 1;
    public int indexPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveNumber);
            SpawnPrefab();
            waveNumber++;
        }
    }

    void SpawnPrefab()
    {
        indexPrefab = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[indexPrefab], GenerateSpawnPosition(), powerUpPrefabs[indexPrefab].transform.rotation);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        int index = Random.Range(0, enemyPrefab.Length);
        int enemies;

        if (index == 0)
        {
            enemies = enemiesToSpawn;
        }
        else 
        {
            enemies = (int)(enemiesToSpawn * 1.25);
        }

        for (int i = 0; i < enemies; i++)
        {
            Instantiate(enemyPrefab[index], GenerateSpawnPosition(), enemyPrefab[index].transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }
}
