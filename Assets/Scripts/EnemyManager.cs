using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    protected PlayerManager player;
    public float health = 100;
    public float defenseMultiplier = 1;
    public GameObject damagePopup;
    float invincibilityTimer = 0.5f;
    public int type = 0;
    public Collider movementCollider;
    public Collider hitboxCollider;
    public float KnockbackStrength = 20f;
    public float attackDmg = 10;
    public float attackRate = 1;

    // MOVEMENT
    public float Speed = 1;
    public Vector3 Displacement;
    public Vector3 Velocity;
    public Vector3 Acceleration;
    public float gravity;
    public bool grounded = false;

    // OTHER
    private bool knockedBackPlayer = false;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        
    }

    public virtual void FixedUpdate()
    {
        CheckAttacks();
        bool col = CollideWithPlayer();

        if(!col)
            knockedBackPlayer = false;

        if (health <= 0)
        {
            DestroyEnemy();
        }
    }

    void CheckAttacks() {
        GameObject playerAttackBox = player.AttackBox;
        if(player.attacking) {
            // Get the player's attack box
            Collider atkCollider = playerAttackBox.GetComponent<BoxCollider>();

            // Check if the player's attack box is colliding with the enemy's attack box
            if (atkCollider.bounds.Intersects(hitboxCollider.bounds) && invincibilityTimer > 0.5f)
            {
                // If the player's attack box is colliding with the enemy's attack box, deal damage to the enemy
                health -= player.DoDamage() / defenseMultiplier;
                DamagePopup dmg = Instantiate(damagePopup, transform.position, Quaternion.identity).GetComponent<DamagePopup>();
                dmg.SetDamage(player.DoDamage() / defenseMultiplier);

                invincibilityTimer = 0;
                Knockback(player.realStats.knockback, -transform.forward);
            }

            invincibilityTimer += Time.deltaTime;
        
        }
    }

    private void Knockback(float knockback, Vector3 direction) {
        Velocity += direction * knockback;
    }

    void DestroyEnemy() {
        player.KilledEnemy(type);
        Destroy(gameObject);
    }

    //knockback player if collide with enemy.
    bool CollideWithPlayer(){
        BoxCollider playerCollider = player.playerCollider;
        if(playerCollider.bounds.Intersects(movementCollider.bounds)) {
            if (!knockedBackPlayer)
            {
                Vector3 KnockBackDir = player.transform.position - transform.position;
                KnockBackDir.Normalize();
                player.Velocity += KnockBackDir * KnockbackStrength;
                knockedBackPlayer = true;
            }
            return true;
        }
        return false;
    }
}
