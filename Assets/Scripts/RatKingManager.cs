using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatKingManager : EnemyManager
{

    public GameObject ratMinion;
    public float minionTimer = 0f;
    public float minionSpawnTime = 5f;
    float totalTimeAlive = 0f;

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Acceleration = Vector3.zero;
        Attack();

        MoveTowardsPlayer();

        DoGravity();
        DoMovement();

        minionTimer += Time.deltaTime;

        if (minionTimer >= minionSpawnTime) {
            SpawnRatMinion();
            minionTimer = 0f;
        }

        totalTimeAlive += Time.deltaTime;
        minionSpawnTime = 5f - totalTimeAlive/60f;
    }

    void SpawnRatMinion() {
        GameObject rat = Instantiate(ratMinion, transform.position, Quaternion.identity);
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
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 20.0f*Time.deltaTime);

        // Set the enemy's rotation to the rotation from the enemy to the player, but only rotate around the y-axis
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        if(Vector3.Dot(Velocity, transform.forward) < Speed && Vector3.Dot(transform.forward, direction) > 0.5f)
            Velocity += transform.forward.normalized * Speed;
        
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

    protected override void DestroyEnemy()
    {
        // get ui manager
        UIManager ui = GameObject.Find("Game Manager").GetComponent<UIManager>();
        // call the boss defeated function
        ui.BossDefeated();
        
        base.DestroyEnemy();
    }

    //enemy attack
    void Attack(){
        // Collider enemyAttackBox = enemyCollider;
        // BoxCollider playerCollider = player.playerCollider;
        // if(playerCollider.bounds.Intersects(enemyAttackBox.bounds) && !player.invincible){
        //     player.TakeDamage(attackDmg);
        //     player.invincible = true;
        // }
    }

}
