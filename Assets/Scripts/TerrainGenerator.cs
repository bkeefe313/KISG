using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    public GameObject Terrain;
    public Gradient gradient;
    // Start is called before the first frame update
    void Start()
    {
        // generate perlin noise terrain data
        gen(1000, 1000, 25, -500, -500);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gen(int width, int height, float scale, float offsetX, float offsetY)
    {
        Terrain = new GameObject("Terrain");
        Terrain.transform.parent = this.transform;
        Terrain.transform.position = new Vector3(0, 0, 0);
        Terrain.transform.rotation = Quaternion.identity;
        Terrain.transform.localScale = new Vector3(1, 1, 1);
        Terrain.AddComponent<MeshFilter>();
        Terrain.AddComponent<MeshRenderer>();
        Terrain.AddComponent<MeshCollider>();
        Mesh mesh = Terrain.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = new Vector3[width * height];
        int[] triangles = new int[(width - 1) * (height - 1) * 6];
        int t = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float x = (float)i / width * scale + offsetX;
                float y = (float)j / height * scale + offsetY;
                float z = Mathf.PerlinNoise(x, y);
                vertices[i * height + j] = new Vector3(i, z * 10, j);
                if (i < width - 1 && j < height - 1)
                {
                    triangles[t] = i * height + j;
                    triangles[t + 1] = i * height + j + 1;
                    triangles[t + 2] = (i + 1) * height + j;
                    triangles[t + 3] = (i + 1) * height + j;
                    triangles[t + 4] = i * height + j + 1;
                    triangles[t + 5] = (i + 1) * height + j + 1;
                    t += 6;
                }
            }
        }

        // Create color map
        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float h = Mathf.InverseLerp(0, 10, vertices[y * width + x].y);
                colorMap[y * width + x] = gradient.Evaluate(h);
            }
        }

        // Create texture
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(colorMap);
        texture.Apply();

        // Apply texture to terrain
        Terrain.GetComponent<MeshRenderer>().material.mainTexture = texture;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        Terrain.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
