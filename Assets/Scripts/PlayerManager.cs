using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // INSTANCE
    static PlayerManager instance;

    // STATS
    public Stats baseStats;
    public Stats realStats;
    public float health;

    // OTHER
    UIManager uimanager;
    public bool attacking = false;
    public GameObject AttackBox;
    public GameObject weapon;
    public bool handbraking = false;
    float timeSinceFreeze = 0;
    public GameObject hitFX;
    public bool boosting = false;
    public float invincibilityTimer = 0;
    public bool invincible = false;

    // CONTROLLER
    CharacterController Controller;
    public Vector3 Acceleration;
    public Vector3 Velocity;
    public float Rotation;
    public float RotationSpeed;
    Vector3 hbForward = new Vector3(0, 0, 1);
    public bool grounded;
    public BoxCollider playerCollider;
    public Rigidbody playerRB;

    // WORLD
    public GameObject Terrain;
    public Transform Cam;
    public PlayerInventory inventory;

    public Terrain terrain;
    public float heightChange;
    public float currentHeight;
    public ParticleSystem RocketBoostFX;

    private void Awake()
    {
        // start of new code
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        uimanager = GameObject.Find("Game Manager").GetComponent<UIManager>();
        uimanager.playerManager = this;
        Controller = GetComponent<CharacterController>();
        inventory = GetComponent<PlayerInventory>();
        playerCollider = GetComponent<BoxCollider>();
        playerRB = GetComponent<Rigidbody>();
        RocketBoostFX = GetComponent<ParticleSystem>();
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();

        if (scene.name == "Level2")
        {
            transform.position = new Vector3(200, 2, 200);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        baseStats = new Stats();
        realStats = new Stats();

        UpdateStats();

        health = realStats.maxHealth;
    }

    void FixedUpdate() {
        DoGravity();
        ApplyMovement();
        if (invincible)
            invincibilityTimer += Time.deltaTime;
        if (invincibilityTimer > 1f) 
        {
            invincible = false;
            invincibilityTimer = 0;
        }
    }

    void Update()
    {
        PollInputs();
        if(Time.timeScale == 0) 
            return;
        Acceleration = Vector3.zero;
        Attack();
        Handbrake();
        PollMovement();
        Cheats();
        RunItems();
        RespawnIfDead();
        BoostFX();
    }

    void PollInputs() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uimanager.TogglePauseMenu();
        }
    }

    void BoostFX() {
        if (boosting) {
            RocketBoostFX.Play();
        } else {
            RocketBoostFX.Stop();
        }
    }

    void RespawnIfDead() {
        if (health <= 0) {
            health = realStats.maxHealth;
            transform.position = new Vector3(0, 10, 0);
        }
        if (transform.position.y < -10) {
            health = realStats.maxHealth;
            transform.position = new Vector3(0, 10, 0);
        }
    }

    void RunItems() {
        foreach (Item item in inventory.inventory) {
            item.Run();
        }
    }

    void Cheats() {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            inventory.GetCommonItem();
        if(Input.GetKeyDown(KeyCode.Alpha2))
            inventory.GetRareItem();
        if(Input.GetKeyDown(KeyCode.Alpha3))
            inventory.GetLegendaryItem();
        if(Input.GetKeyDown(KeyCode.Alpha4))
            SceneManager.LoadScene("Level2");
    }

    void ApplyMovement() {
        // apply acceleration
        Vector3 xzVelocity = new Vector3(Velocity.x, 0, Velocity.z);
        if (xzVelocity.magnitude > realStats.speed && !boosting)
            Velocity += Acceleration / Mathf.Pow(xzVelocity.magnitude / realStats.speed,2);
        else
            Velocity += Acceleration;

        // rotate velocity vector with rotation
        Vector3 xzVel = new Vector3(Velocity.x, 0, Velocity.z);
        Vector3 xzRot = Quaternion.Euler(0, Rotation*Time.deltaTime, 0) * xzVel;
        Velocity = new Vector3(xzRot.x, Velocity.y, xzRot.z);

        // Get incoming height change
        heightChange = terrain.SampleHeight(transform.position + Velocity * Time.deltaTime) - currentHeight;
        if(heightChange < 0)
            heightChange = 0;
        
        // scale velocity down based on height change
        Velocity /= Mathf.Pow(heightChange/10 + 1, 2);

        // move player
        transform.position += Velocity * Time.deltaTime + new Vector3(0, heightChange, 0);

        transform.Rotate(0, Rotation*(handbraking ? realStats.handbrakeMultiplier : 1)*Time.deltaTime, 0);
    }

    void PollMovement() {
        Vector3 dir = transform.forward;
        Vector3 xzMovement = new Vector3(Velocity.x, 0, Velocity.z);
        currentHeight = terrain.SampleHeight(transform.position);

        // handbrake keeps direction of movement constant while turning
        if (Velocity.magnitude < 0.1f) {
            hbForward = transform.forward;
        } else {
            // increment dir from transform.forward to hbForward
            dir = Vector3.RotateTowards(dir, hbForward, 0.01f, 0);
        }

        if(Input.GetKey(KeyCode.W))
        {   
            if(Velocity.magnitude > 0.1f && Vector3.Dot(Velocity, -transform.forward) > 0)
                Acceleration = transform.forward * realStats.speedMultiplier * 10;
            else
                Acceleration = transform.forward * realStats.speedMultiplier;
        } else if(Input.GetKey(KeyCode.S))
        {
            if(Velocity.magnitude > 0.1f && Vector3.Dot(Velocity, transform.forward) > 0)
                Acceleration = -transform.forward * realStats.speedMultiplier * Velocity.magnitude;
            else
                Acceleration = -transform.forward * realStats.speedMultiplier;
        } else
        {
            if (Mathf.Abs(Velocity.magnitude) < realStats.speedMultiplier*2) {
                Velocity = new Vector3(0, Velocity.y, 0);
            } else 
            {
                Acceleration = -xzMovement / 10f;
            }
        }

        float rotFactor = 1f / (1f + (Mathf.Pow(Velocity.magnitude, 0.1f)));
        if(Input.GetKey(KeyCode.A))
        {
            Rotation = -RotationSpeed/10 * rotFactor;
        } else if(Input.GetKey(KeyCode.D))
        {
            Rotation = RotationSpeed/10 * rotFactor;
        } else
        {
            Rotation = 0;
        }
        Rotation = Mathf.Clamp(Rotation, -RotationSpeed, RotationSpeed);

        // jump
        if (grounded) {
            if(Input.GetKey(KeyCode.Space))
                Velocity.y = realStats.jumpForce;
            else
                Velocity.y = 0;
        }

        // Visibly tilt cart with speed and turning amount
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, Rotation * Velocity.magnitude * (Mathf.Sqrt(RotationSpeed)/10000));
    }

    void DoGravity() {
        if (!grounded)
            Velocity.y += realStats.gravity * Time.deltaTime;

        int layerMask = 1 << 3;
        layerMask = ~layerMask;

        // RaycastHit hit;
        // grounded = Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f, layerMask);
        // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);

        currentHeight = terrain.SampleHeight(transform.position);
        float diff = transform.position.y - currentHeight;
        grounded = diff < 0.1f;
        if (diff < 0)
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
    }

    void Handbrake() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && realStats.handbrakeMultiplier > 0)
        {
            handbraking = true;
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            handbraking = false;
        }
    }

    void Attack() {
        if (Input.GetMouseButtonDown(0) && weapon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idlesword") && !attacking)
        {
            // play animation
            weapon.GetComponent<Animator>().SetBool("Attacking", true);
        }
        else if (weapon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idlesword"))
        {
            weapon.GetComponent<Animator>().SetBool("Attacking", false);
        }

        if(Time.timeScale != 1 && timeSinceFreeze < 0.05f) {
            timeSinceFreeze += Time.unscaledDeltaTime;
        } else if(Time.timeScale != 1) {
            Time.timeScale = 1;
        }
    }

    public void KilledEnemy(int type) {
        inventory.money += type * 10;
        uimanager.UpdateMoney(inventory.money);
    }
    

    public float DoDamage() {
        Time.timeScale = 0.05f;
        timeSinceFreeze = 0;

        float damage = realStats.baseDamage * realStats.attackMultiplier * (0.1f + (Velocity.magnitude / realStats.speed));

        // Damage Particle Effect
        GameObject fx = Instantiate(hitFX, weapon.transform.position, Quaternion.identity);
        // scale fx by damage 
        fx.transform.localScale = new Vector3(1, 1, 1) * damage/50;
        Destroy(fx, 1.0f);

        return damage;
    }

    public void UpdateStats() {
        // reset to base stats
        realStats = new Stats(baseStats);
        // apply item effects
        foreach (Item item in inventory.inventory) {
            for(int i = item.count; i > 0; i--) {
                item.UpdateStats(this);
            }
        }
        weapon.GetComponent<Animator>().SetFloat("AttackSpeedMult", realStats.attackSpeed);
    }

    void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag == "Projectile" && !invincible) {
            other.gameObject.GetComponent<SlimeProjectile>().Destroy(this);
        }
    }

    public void TakeDamage(float damage) {
        health -= damage / realStats.defenseMultiplier;
        // screen effect
        uimanager.ActivateDamageFX();
    }
}
