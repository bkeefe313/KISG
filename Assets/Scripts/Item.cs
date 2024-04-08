using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    public string itemname;
    public int rarity;
    public int id;
    public int count;
    public GameObject Popup;
    public GameObject model;
    public ItemEffectContainer effects;
    public ItemRunnable runnable;
    public Sprite icon;

    public void Init() {
        effects = gameObject.GetComponent<ItemEffectContainer>();
        if (id == 4)
        {
            runnable = new RocketBooster();
            runnable.Init(ref count);
        }
        // Burnt Turkey
        if (id == 100)
        {
            runnable = new TurkeyHeals();
            runnable.Init(ref count);
        }
        // Printer
        if (id == 101)
        {
            runnable = new Printer();
            runnable.Init(ref count);
        }
    }

    public void MakePopup() 
    {
        Popup.GetComponentInChildren<RawImage>().texture = icon.texture;
        Debug.Log("You picked up: " + itemname);
        Instantiate(Popup, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void UpdateStats(PlayerManager player) {
        if (effects == null)
        {
            effects = gameObject.GetComponent<ItemEffectContainer>();
        }
        player.realStats.maxHealth += effects.health;
        player.realStats.speed += effects.speed;
        player.realStats.speedMultiplier += effects.speedMultiplier;
        player.realStats.jumpForce += effects.jumpForce;
        player.realStats.baseDamage += effects.baseDamage;
        player.realStats.attackMultiplier += effects.attackMultiplier;
        player.realStats.defenseMultiplier += effects.defenseMultiplier;
        player.realStats.attackSpeed += effects.attackSpeed;
        player.realStats.gravity += effects.gravity;
        player.realStats.handbrakeMultiplier += effects.handbrakeMultiplier;
        player.realStats.knockback += effects.knockback;
        player.inventory.money += effects.money;
    }

    public void Run()
    {
        if (runnable != null)
        {
            runnable.Run();
        }
    }

}
