using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CameraMove : MonoBehaviour
{
 
    private const float YMin = -50.0f;
    private const float YMax = 50.0f;
 
    public Transform lookAt;
 
    public Transform Player;
 
    public float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float sensivity = 4.0f;
 
 
    // Start is called before the first frame update
    void Start()
    {
     
        // hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Player = GameObject.Find("Player").transform;
        lookAt = GameObject.Find("Player").transform.GetChild(0).transform;
 
    }
 
    // Update is called once per frame
    void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;
 
        currentY = Mathf.Clamp(currentY, YMin, YMax);
 
        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(0, currentX, 0);
        rotation = rotation * Quaternion.Euler(currentY, Player.eulerAngles.y, 0);
        transform.position = lookAt.position + rotation * Direction;
        distance += Input.GetAxis("Mouse ScrollWheel") * 2;
        if (distance < 2)
        {
            distance = 2;
        }
        if (distance > 80)
        {
            distance = 80;
        }
 
        transform.LookAt(lookAt.position);
    }
}