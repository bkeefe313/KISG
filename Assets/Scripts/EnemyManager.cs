using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    protected PlayerManager playerManager;
    public float health = 100;
    public float defenseMultiplier = 1;
    public GameObject damagePopup;
    float invincibilityTimer = 0.5f;
    public int type = 0;

    // MOVEMENT
    public float Speed = 1;
    public Vector3 Displacement;
    public Vector3 Velocity;
    public Vector3 Acceleration;
    public float gravity;
    public bool grounded = false;

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    public virtual void FixedUpdate()
    {
        CheckAttacks();

        if (health <= 0)
        {
            DestroyEnemy();
        }
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
}
