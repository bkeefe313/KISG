using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);

        foreach (var hitCollider in hitColliders) {
            if (hitCollider.gameObject.tag.Equals("Dumpster")) {
                GameObject dumpster = hitCollider.gameObject;
                dumpster.GetComponent<Animator>().SetBool("Opened", true);
            }
        }
    }
}
