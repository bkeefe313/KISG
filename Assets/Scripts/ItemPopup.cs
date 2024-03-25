using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopup : MonoBehaviour
{
    float timer = 0;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        // get player
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Draw the item popup above the player's head
        transform.position = player.transform.position + new Vector3(0, 2, 0);
        transform.LookAt(player.transform);

        timer += Time.deltaTime;
        if (timer > 3)
        {
            Destroy(gameObject);
        }
    }
}
