using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpsterHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Dumpster collided with player");
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Dumpster collided with player");
            GameObject player = GameObject.Find("Player");
            GameObject dumpster = GameObject.Find("Dumpster");
            player.transform.position = new Vector3(dumpster.transform.position.x, dumpster.transform.position.y + 1, dumpster.transform.position.z);
        }
    }
}
