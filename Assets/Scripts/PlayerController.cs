using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerMove : MonoBehaviour
{
 
 
    CharacterController Controller;
 
    public float Acceleration;
    public float MaxSpeed;
    public float RotationSpeed;
    public float MaxRotationSpeed;
    float Rotational;
    float Forward;
 
    public Transform Cam;
 
 
    // Start is called before the first frame update
    void Start()
    {
 
        Controller = GetComponent<CharacterController>();
 
    }
 
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            Forward += Acceleration;
        } else if(Input.GetKey(KeyCode.S))
        {
            Forward -= Acceleration;
        } else
        {
            if (Mathf.Abs(Forward) < Acceleration)
                Forward = 0;
            else
                Forward -= Mathf.Sign(Forward) * Acceleration;
        }
        Forward = Mathf.Clamp(Forward, -MaxSpeed, MaxSpeed);

        // Get Rotation
        if(Input.GetKey(KeyCode.A))
        {
            Rotational = -RotationSpeed * Time.deltaTime * (Forward < 1 ? 0 : (1/Mathf.Sqrt(Forward)));
        } else if(Input.GetKey(KeyCode.D))
        {
            Rotational = RotationSpeed * Time.deltaTime * (Forward < 1 ? 0 : (1/Mathf.Sqrt(Forward)));
        } else
        {
            Rotational = 0;
        }
        Rotational = Mathf.Clamp(Rotational, -MaxRotationSpeed, MaxRotationSpeed);

        // Create movement vector
        Vector3 Movement = transform.forward * Forward * Time.deltaTime;
        Movement.y = 0f;

        // Clamp movement
        Mathf.Clamp(Movement.x, -1, 1);
        Mathf.Clamp(Movement.x, -1, 1);

        // Apply movement
        Controller.Move(Movement);
        transform.Rotate(0, Rotational, 0);
    }

}