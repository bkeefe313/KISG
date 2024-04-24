using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lvl2trigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        Scene scene = SceneManager.GetActiveScene();
        if(other.tag == "Player") {
            if (scene.buildIndex == 1) {
                SceneManager.LoadScene(0);
            } else {
                SceneManager.LoadScene(1);
            }
        }
    }
}
