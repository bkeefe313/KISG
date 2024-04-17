using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : ItemRunnable
{
    private PlayerManager player;

    public override void Init(ref int c)
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        count = c;
    }

    public override void Run()
    {
        if(count > 0)
        {
            foreach(Item i in player.inventory.inventory)
            {
                if(i.id == item.id)
                    continue;
                if(i.count > 0)
                    i.count*=2;
            }
            count = 0;
        }
    }
}
