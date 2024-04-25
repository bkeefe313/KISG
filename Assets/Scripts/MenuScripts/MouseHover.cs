using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update


    // void Start()
    // {
    //     gameObject.GetComponent<Renderer>().material.color = Color.white;
    // }


    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red; //Or however you do your color
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white; //Or however you do your color
    }



    // void OnMouseEnter(){
    //     GetComponent<Renderer>().material.color = Color.blue;

    // }

    // void OnMouseExit(){
    //     GetComponent<Renderer>().material.color = Color.white;

    // }


    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
