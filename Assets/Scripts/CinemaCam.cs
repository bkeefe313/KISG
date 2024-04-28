using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaCam : MonoBehaviour
{
    public Camera playerCam;
    public Camera cinemaCam;

    // Start is called before the first frame update
    void Start()
    {
        playerCam.enabled = true;
        cinemaCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)) {
            playerCam.enabled = !playerCam.enabled;
            cinemaCam.enabled = !cinemaCam.enabled;
        }
    }
}
