using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerManager manager;

    void Start()
    {
        manager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }
    void OnGUI()
    {
        GUI.Box(new Rect(40, 5, 1000, 90), "Player Stats");
        GUI.Label(new Rect(50, 20, 900, 20), "Health: " + manager.realStats.health);
        GUI.Label(new Rect(50, 40, 900, 20), "Money: " + manager.money);
        GUI.Label(new Rect(50, 60, 900, 20), "Inventory: " + inventoryString(manager.inventory));
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
        manager.money = amount;
    }
}
