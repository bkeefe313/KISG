using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    PlayerManager playerManager;
    public float health = 100;
    public float defenseMultiplier = 1;
    public GameObject damagePopup;
    float invincibilityTimer = 0.5f;
    public int type = 0;
    public float gravity;
    public bool grounded = false;

    // MOVEMENT
    public float Speed = 1;
    public Vector3 Displacement;
    public Vector3 Velocity;
    public Vector3 Acceleration;

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    public void FixedUpdate()
    {
        Acceleration = Vector3.zero;

        if (CanSeePlayer())
            MoveTowardsPlayer();
        CheckAttacks();

        DoGravity();
        DoMovement();

        if (health <= 0)
        {
            DestroyEnemy();
        }
    }

    bool CanSeePlayer() 
    {
        GameObject player = GameObject.Find("Player");
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

    void CheckAttacks() {
        GameObject playerAttackBox = playerManager.AttackBox;
        if(playerManager.attacking) {
            // Get the player's attack box
            BoxCollider playerCollider = playerAttackBox.GetComponent<BoxCollider>();

            // Get the enemy's attack box
            BoxCollider enemyCollider = GetComponent<BoxCollider>();

            // Check if the player's attack box is colliding with the enemy's attack box
            if (playerCollider.bounds.Intersects(enemyCollider.bounds) && invincibilityTimer > 0.5f)
            {
                // If the player's attack box is colliding with the enemy's attack box, deal damage to the enemy
                health -= playerManager.GetDamage() / defenseMultiplier;
                DamagePopup dmg = Instantiate(damagePopup, transform.position, Quaternion.identity).GetComponent<DamagePopup>();
                dmg.SetDamage(playerManager.GetDamage() / defenseMultiplier);

                invincibilityTimer = 0;
                Knockback(playerManager.realStats.knockback, -transform.forward);
            }

            invincibilityTimer += Time.deltaTime;
        
        }
    }

    private void Knockback(float knockback, Vector3 direction) {
        Velocity += direction * knockback;
    }

    void DestroyEnemy() {
        playerManager.KilledEnemy(type);
        Destroy(gameObject);
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
