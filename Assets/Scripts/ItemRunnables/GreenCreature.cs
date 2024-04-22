using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCreature : ItemRunnable
{
    private PlayerManager player;
    override public void Init(Item i)
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        item = i;
    }

    override public void Run()
    {
        if(item.count > 0)
        {
            player.inventory.money = 0;
            item.count = 0;
        }
    }
}
