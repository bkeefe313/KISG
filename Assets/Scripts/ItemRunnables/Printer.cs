using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : ItemRunnable
{
    private PlayerManager player;

    public override void Init(Item i)
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        item = i;
    }

    public override void Run()
    {
        if(item.count > 0)
        {
            foreach(Item i in player.inventory.inventory)
            {
                if(i.id == item.id)
                    continue;
                if(i.count > 0)
                    i.count*=2;
            }
            item.count = 0;
        }
    }
}
