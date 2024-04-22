using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurkeyHeals : ItemRunnable
{
    private PlayerManager player;
    private float healAmount = 10;
    float delay = 10;
    float sinceLastHeal = 0;

    public TurkeyHeals(){}

    override public void Init(Item i)
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        item = i;
    }

    override public void Run()
    {
        if(sinceLastHeal >= delay)
        {
            player.health += healAmount * item.count;
            if (player.health > player.realStats.maxHealth)
            {
                player.health = player.realStats.maxHealth;
            }
            sinceLastHeal = 0;
        }
        sinceLastHeal += Time.deltaTime;
    }
}
