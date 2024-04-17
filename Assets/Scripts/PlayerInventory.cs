using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // ITEM CONSTANTS
    int num_common_items;
    int num_rare_items;
    int num_legendary_items;

    // INVENTORY
    public List<Item> inventory;
    public int money;
    PlayerManager playerManager;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        num_common_items = 0;
        num_rare_items = 0;
        num_legendary_items = 0;
        foreach (Item item in inventory) {
            item.count = 0;
            if (item.rarity == 0) {
                num_common_items++;
            } else if (item.rarity == 1) {
                num_rare_items++;
            } else if (item.rarity == 2) {
                num_legendary_items++;
            }
            item.Init();
        }
    }

    public void GetCommonItem() {
        int item = UnityEngine.Random.Range(0, num_common_items);
        inventory[item].count+=1;
        inventory[item].MakePopup();
        playerManager.UpdateStats();
    }
    public void GetRareItem() {
        int item = UnityEngine.Random.Range(num_common_items, num_common_items+num_rare_items);
        inventory[item].count+=1;
        inventory[item].MakePopup();
        playerManager.UpdateStats();
    }
    public void GetLegendaryItem() {
        int item = UnityEngine.Random.Range(num_common_items+num_rare_items, num_common_items+num_rare_items+num_legendary_items);
        Debug.Log("Item: " + item);
        inventory[item].count+=1;
        inventory[item].MakePopup();
        playerManager.UpdateStats();
    }
}
