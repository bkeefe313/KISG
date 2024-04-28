using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable] public class ChestSpawner : MonoBehaviour
{
    public GameObject rareChest;
    public GameObject mediumChest;
    public GameObject commonChest;
    public GameObject Spire;
    public GameObject Portal;
    public float heightLimit;
    public GameObject Terrain;

    void Start()
    {
        Terrain = GameObject.Find("Terrain");
        // Get the terrain dimensions
        Terrain terrain = Terrain.GetComponent<Terrain>();
        float terrainWidth = terrain.terrainData.size.x * 0.75f;
        float terrainLength = terrain.terrainData.size.z * 0.75f;

        // Create a list to store the spawn points
        List<Vector3> spawnPoints = new List<Vector3>();

        // Determine the number of spawn points you want to generate
        int numberOfSpawnPoints = 25;

        // Generate spawn points
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            float x = 0f;
            float z = 0f;
            float y = -1f;
            while (y > heightLimit || y < 0f)
            {
                x = Random.Range(-terrainWidth / 2, terrainWidth / 2);
                z = Random.Range(-terrainLength / 2, terrainLength / 2);
                y = terrain.SampleHeight(new Vector3(x, 0, z));
            }

            // Add the spawn point to the list
            spawnPoints.Add(new Vector3(x, y, z));
        }

        SpawnChests(spawnPoints);
        if (SceneManager.GetActiveScene().name == "Level1")
            SpawnObject(terrainWidth, terrainLength, terrain, Spire);
    }

    void SpawnObject(float terrainWidth, float terrainLength, Terrain terrain, GameObject obj, Vector3 offset = new Vector3())
    {
        float x = 0f;
        float z = 0f;
        float y = -1f;
        while (y > heightLimit || y < 0f)
        {
            x = Random.Range(-terrainWidth / 2, terrainWidth / 2);
            z = Random.Range(-terrainLength / 2, terrainLength / 2);
            y = terrain.SampleHeight(new Vector3(x, 0, z));
        }

        Instantiate(obj, new Vector3(x, y, z) + offset, Quaternion.identity);
    }

    void SpawnChests(List<Vector3> spawnPoints)
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            int random = Random.Range(0, 100);
            int rarity = random < 50 ? 0 : random < 80 ? 1 : 2;
            Vector3 spawnPoint = spawnPoints[i];
            GameObject chest = Instantiate(rarity == 0 ? commonChest : rarity == 1 ? mediumChest : rareChest, spawnPoint, Quaternion.identity);
            chest.GetComponent<ChestHandler>().rarity = rarity;
        }
    }
}
