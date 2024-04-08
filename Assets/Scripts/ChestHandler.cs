using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestHandler : MonoBehaviour
{

    PlayerManager manager;
    PlayerInventory inventory;
    UIManager uimanager;
    public int rarity;
    public float chestOpenDistance = 15f;


    void Start()
    {
        manager = GameObject.Find("Player").GetComponent<PlayerManager>();
        uimanager = GameObject.Find("Game Manager").GetComponent<UIManager>();
        inventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
    }

    void OpenChest() {
        // determine chest contents
        int random = Random.Range(0, 100);

        int lowerBound = 0;
        int upperBound = 0;

        switch (rarity)
        {
            case 0:
                lowerBound = 75;
                upperBound = 99;
                break;
            case 1:
                lowerBound = 50;
                upperBound = 85;
                break;
            case 2:
                lowerBound = 20;
                upperBound = 60;
                break;
            default:
                lowerBound = 100;
                upperBound = 101;
                break;
        }
        if (random < lowerBound) {
            // Add common item to inventory
            inventory.GetCommonItem();
        } else if (random < upperBound) {
            // Add rare item to inventory
            inventory.GetRareItem();
        } else {
            // Add legendary item to inventory
            inventory.GetLegendaryItem();
        }
    }

    void Update()
    {
        // Get the player's position
        Vector3 playerPosition = GameObject.Find("Player").transform.position;

        // Get the chest's position
        Vector3 chestPosition = transform.position;

        // Calculate the distance between the player and the chest
        float distance = Vector3.Distance(playerPosition, chestPosition);

        // If the player is close enough to the chest, open it
        if (distance < chestOpenDistance && Input.GetKeyDown(KeyCode.E) && inventory.money >= rarity*100)
        {
            OpenChest();
            inventory.money -= rarity*100;
            Destroy(gameObject);
        }
    }

    void OnGUI()
    {
        // If the player is close enough to the chest, display a message
        if (Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) < chestOpenDistance)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100), "Chest Cost: " + rarity*100);
        }
    }
}
