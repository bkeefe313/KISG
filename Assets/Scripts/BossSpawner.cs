using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSpawner : MonoBehaviour
{
    public GameObject boss1;
    public GameObject boss2;
    public GameObject player;
    private Renderer rend;
    public bool destroy = false;

    void Start() 
    {
        player = GameObject.Find("Player");
        rend = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if player is near the boss spawner
        float playerDist = Vector3.Distance(player.transform.position, transform.position);
        if(playerDist < 10)
        {
            // if player presses E
            if(Input.GetKeyDown(KeyCode.E))
            {
                // spawn the boss
                SpawnBoss();
                // destroy the spawner
                destroy = true;
            }
        }

        if(destroy)
        {
            Color c = rend.material.color;
            c.a -= 0.01f;
            rend.material.color = c;
            if(c.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void SpawnBoss() {
        if(SceneManager.GetActiveScene().name == "Level1")
            Instantiate(boss1, transform.position, Quaternion.identity);
        else if(SceneManager.GetActiveScene().name == "Level2")
            Instantiate(boss2, transform.position, Quaternion.identity);
    }

}
