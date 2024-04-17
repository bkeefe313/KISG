using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public PlayerInventory playerInventory;
    public bool paused = false;

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
    }
    void OnGUI()
    {
        GUI.Box(new Rect(40, 5, 1000, 90), "Player Stats");
        GUI.Label(new Rect(50, 20, 900, 20), "Health: " + playerManager.health + "/" + playerManager.realStats.maxHealth);
        GUI.Label(new Rect(50, 40, 900, 20), "Money: " + playerInventory.money);
        GUI.Label(new Rect(50, 60, 900, 20), "Inventory: " + inventoryString(playerInventory.inventory));
        if(paused)
        {
            GUI.Box(new Rect(500, 200, 200, 100), "Pause Menu");
            if (GUI.Button(new Rect(550, 220, 100, 20), "Resume"))
            {
                TogglePauseMenu();
            }
            if (GUI.Button(new Rect(550, 240, 100, 20), "Quit"))
            {
                Application.Quit();
            }
        }
    }
    string inventoryString(List<Item> inventory)
    {
        string result = "";
        foreach (Item item in inventory)
        {
            if(item.count > 0)
                result += item.itemname + " x" + item.count + ", ";
        }
        return result;
    }

    public void UpdateMoney(int amount)
    {
        playerInventory.money = amount;
    }

    public void TogglePauseMenu()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
    }
}
