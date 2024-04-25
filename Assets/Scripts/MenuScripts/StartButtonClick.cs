using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class StartButtonClick : MonoBehaviour
{
   
    [SerializeField] private string nextGameLevel = "Level1";

	public void onClick(){

		Debug.Log ("You have clicked the button!");
        SceneManager.LoadScene(nextGameLevel);
	}
  


}
