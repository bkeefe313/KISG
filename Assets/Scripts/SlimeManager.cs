using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : EnemyManager
{

    public float FiringRange = 10;
    public GameObject Projectile;
    float TimeSinceFired = 0;
    public float FireDelay = 2;

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Acceleration = Vector3.zero;

        if (CanSeePlayer() && !PlayerInRange())
            MoveTowardsPlayer();
        else
            Velocity = Vector3.zero;

        if (PlayerInRange())
            DoAttacks();

        DoGravity();
        DoMovement();
    }

    void DoAttacks()
    {
        Quaternion firingDir = Quaternion.LookRotation(player.transform.position - transform.position);
        Vector3 firingDirVec = firingDir.eulerAngles.normalized;

        if (TimeSinceFired > FireDelay)
        {
            GameObject proj = Instantiate(Projectile, transform.position + firingDirVec + Vector3.up * 3, firingDir);
            TimeSinceFired = 0;
        }

        TimeSinceFired += Time.deltaTime;
    }

    bool PlayerInRange()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 enemyPosition = transform.position;

        return Vector3.Distance(playerPosition, enemyPosition) < FiringRange;
    }

    bool CanSeePlayer() 
    {
        Vector3 playerPosition = player.transform.position;

        // Enemies are on layer 6 (ignore)
        int layerMask = 1 << 6;
        layerMask = ~layerMask;

        RaycastHit hit;
        Physics.Raycast(transform.position, playerPosition - transform.position, out hit, Mathf.Infinity, layerMask);

        if (hit.collider != null && hit.collider.gameObject.tag == "Player")
        {
            return true;
        }
        return false;
    }

    void MoveTowardsPlayer()
    {
        // Get the player
        GameObject player = GameObject.Find("Player");

        // Get the player's position
        Vector3 playerPosition = player.transform.position;

        // Get the enemy's position
        Vector3 enemyPosition = transform.position;

        // Get the direction from the enemy to the player
        Vector3 direction = playerPosition - enemyPosition;

        // Get the rotation from the enemy to the player
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Set the enemy's rotation to the rotation from the enemy to the player, but only rotate around the y-axis
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        // Move the enemy towards the player
        if (Velocity.normalized != direction.normalized) {
            Velocity -= Velocity.normalized * Speed;
            Velocity += direction.normalized * Speed;
        }

        if(Velocity.magnitude < Speed)
            Velocity += direction.normalized * Speed;
        
    }

    void DoMovement() {
        // Apply acceleration
        Velocity += Acceleration * Time.deltaTime;
        if (grounded)
            Velocity.y = 0;


        Vector3 xzVel = new Vector3(Velocity.x, 0, Velocity.z);
        if (xzVel.magnitude > Speed) {
            xzVel = xzVel.normalized * Speed;
            Velocity = new Vector3(xzVel.x, Velocity.y, xzVel.z);
        }

        // Apply movement
        transform.position += Velocity * Time.deltaTime;
    }

    void DoGravity() {
        if (!grounded)
            Velocity.y += gravity * Time.deltaTime;

        int layerMask = 1 << 6;
        layerMask = ~layerMask;

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask);
        grounded = hit.distance < 0.5f;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
    }
}
