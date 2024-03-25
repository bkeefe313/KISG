using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTracking : MonoBehaviour
{
    public float Speed;
    public GameObject Player;
    float CollisionDistance = 1.0f;

    void MoveTowardsPlayer()
    {
        // Get the player's position
        Vector3 playerPosition = Player.transform.position;

        // Get the enemy's position
        Vector3 enemyPosition = transform.position;

        // Get the direction from the enemy to the player
        Vector3 direction = playerPosition - enemyPosition;

        // Get the rotation from the enemy to the player
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Set the enemy's rotation to the rotation from the enemy to the player
        transform.rotation = rotation;

        // Move the enemy towards the player
        transform.position += transform.forward * Time.deltaTime * Speed;
    }

    void CollideWithRats()
    {
        // Get all the rats in the scene
        GameObject[] rats = GameObject.FindGameObjectsWithTag("Rat");

        // Loop through each rat
        foreach (GameObject other in rats)
        {
            if (other == gameObject)
            {
                continue;
            }
            // Get the rat's position
            Vector3 otherPosition = other.transform.position;

            // Get the enemy's position
            Vector3 myPosition = transform.position;

            // Get the direction from the enemy to the rat
            Vector3 direction = otherPosition - myPosition;

            // Get the distance from the enemy to the rat
            float distance = direction.magnitude;

            // If the enemy is close to the rat
            if (distance < CollisionDistance)
            {
                Debug.Log("Colliding with rat");
                // push each other away
                Vector3 pushDirection = direction.normalized;
                transform.position -= pushDirection * Time.deltaTime * Speed * 5;
                other.transform.position += pushDirection * Time.deltaTime * Speed * 5;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
        CollideWithRats();
    }
}
