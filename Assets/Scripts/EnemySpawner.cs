using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public GameObject Terrain;
    public int numEnemies;
    public float spawnRadius;
    public float spawnRate;
    public float spawnTimer;
    public float heightLimit = 10f;

    void Start()
    {
        spawnTimer = spawnRate;
    }

    void Update()
    {
        if (numEnemies > 0)
        {
            spawnTimer -= Time.deltaTime;
            // Get the terrain dimensions
            Terrain terrain = Terrain.GetComponent<Terrain>();
            float terrainWidth = terrain.terrainData.size.x * 0.75f;
            float terrainLength = terrain.terrainData.size.z * 0.75f;
            float x = 0f;
            float z = 0f;
            float y = -1f;
            while (y > heightLimit || y < 0f)
            {
                x = Random.Range(-terrainWidth / 2, terrainWidth / 2);
                z = Random.Range(-terrainLength / 2, terrainLength / 2);
                y = terrain.SampleHeight(new Vector3(x, 0, z)) + 10f;
            }
            if (spawnTimer <= 0)
            {
                int random = Random.Range(0, enemyPrefabs.Count);
                GameObject enemy = Instantiate(enemyPrefabs[random], new Vector3(x, y, z), Quaternion.identity);
                numEnemies--;
                spawnTimer = spawnRate;
            }
        }
    }
}
