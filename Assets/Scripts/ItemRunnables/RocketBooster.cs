using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RocketBooster : ItemRunnable
{
    private PlayerManager player;
    private float power = 1f;

    public RocketBooster()
    {
        key = KeyCode.C;
    }

    override public void Init(Item i)
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        item = i;
    }
    override public void Run()
    {
        if (Input.GetKey(key) && item.count > 0)
        {
            player.Velocity += player.transform.forward * power * item.count;
            player.boosting = true;
        } else
        {
            player.boosting = false;
        }
    }
}
