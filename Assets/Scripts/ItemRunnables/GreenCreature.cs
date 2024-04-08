using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCreature : ItemRunnable
{
    private PlayerManager player;
    override public void Init(ref int c)
    {
        count = c;
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    override public void Run()
    {
        if(count > 0)
        {
            player.inventory.money = 0;
            count = 0;
        }
    }
}
