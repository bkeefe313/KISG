using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RocketBooster : ItemRunnable
{
    private PlayerManager player;
    private float power = 0.05f;

    public RocketBooster()
    {
        key = KeyCode.C;
    }

    override public void Init(ref int c)
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        count = c;
    }

    override public void Run()
    {
        if (Input.GetKey(key))
        {
            Vector3 xzVel = new Vector3(player.Velocity.x, 0, player.Velocity.z);
            player.Velocity += xzVel.normalized * power * count;
        }
    }
}
