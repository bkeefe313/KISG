using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    public string itemname;
    public int rarity;
    public int id;
    public int count;
    public GameObject Popup;
    public GameObject model;
    public ItemEffectContainer effects;

    void Start() {
        effects = gameObject.GetComponent<ItemEffectContainer>();
    }

    public void MakePopup() 
    {
        Debug.Log("You picked up: " + itemname);
        Instantiate(Popup, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void UpdateStats(PlayerManager player) {
        if (effects == null)
        {
            effects = gameObject.GetComponent<ItemEffectContainer>();
        }
        player.realStats.health += effects.health;
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
        player.money += effects.money;
    }

}
