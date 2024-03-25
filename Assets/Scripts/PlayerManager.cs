using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ITEM CONSTANTS
    int NUM_COMMON_ITEMS = 5;
    int NUM_RARE_ITEMS = 5;
    int NUM_LEGENDARY_ITEMS = 5;
    // STATS
    public Stats baseStats;
    public Stats realStats;

    // INVENTORY
    public List<Item> inventory;
    public int money;

    // OTHER
    UIManager uimanager;
    public bool attacking = false;
    public GameObject AttackBox;
    private float timeSinceLastAttack = 0;
    public GameObject weapon;
    public bool handbraking = false;
    float timeSinceFreeze = 0;
    public GameObject hitFX;

    // CONTROLLER
    CharacterController Controller;
    public Vector3 Acceleration;
    public Vector3 Velocity;
    public float Rotation;
    public float RotationSpeed;
    Vector3 hbForward = new Vector3(0, 0, 1);
    bool grounded;

    // WORLD
    public GameObject Terrain;
    public Transform Cam;

    // Start is called before the first frame update
    void Start()
    {
        uimanager = GameObject.Find("Game Manager").GetComponent<UIManager>();
        Controller = GetComponent<CharacterController>();

        foreach (Item item in inventory) {
            item.count = 0;
        }

        baseStats = new Stats();
        realStats = new Stats();
    }

    void FixedUpdate() {
        DoGravity();

        ApplyMovement();
    }

    void Update()
    {
        Acceleration = Vector3.zero;
        Attack();
        Handbrake();
        DoMovement();
    }

    void ApplyMovement() {
        if (Velocity.magnitude > realStats.speed)
            Velocity = Vector3.ClampMagnitude(Velocity, realStats.speed);
        Velocity += Acceleration;
        Controller.Move(Velocity*Time.deltaTime);
        transform.Rotate(0, Rotation*(handbraking ? realStats.handbrakeMultiplier : 1)*Time.deltaTime, 0);
    }

    void DoMovement() {
        Vector3 dir = transform.forward;

        // handbrake keeps direction of movement constant while turning
        if (!handbraking)
            hbForward = transform.forward;
        else 
            dir = hbForward;

        if(!handbraking) {
            if(Input.GetKey(KeyCode.W))
            {   
                Acceleration = dir * realStats.speedMultiplier;
            } else if(Input.GetKey(KeyCode.S))
            {
                Acceleration = -dir * realStats.speedMultiplier;
            } else
            {
                Vector3 xzMovement = new Vector3(Velocity.x, 0, Velocity.z);
                if (Mathf.Abs(Velocity.magnitude) < realStats.speedMultiplier*2) {
                    Velocity = new Vector3(0, Velocity.y, 0);
                } else {
                    // "friction"
                    Acceleration = -Vector3.Normalize(xzMovement) * realStats.speedMultiplier;
                }
            }
        }

        // slow rotation if moving quickly using magnitude of movement vector
        int sign = Vector3.Dot(Velocity, dir) > 0 ? 1 : -1;
        float rotFactor = sign * (Mathf.Abs(Velocity.magnitude) < 1 ? 0 : (1/Mathf.Sqrt(Mathf.Abs(Velocity.magnitude))));
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
                Velocity.y += realStats.jumpForce;
            else
                Velocity.y = 0;
        }

        // Visibly tilt cart with speed and turning amount
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, Rotation * Velocity.magnitude * (RotationSpeed/100000));
    }

    void DoGravity() {
        if (!grounded)
            Velocity.y += realStats.gravity * Time.deltaTime;

        int layerMask = 1 << 3;
        layerMask = ~layerMask;

        RaycastHit hit;
        grounded = Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f, layerMask);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
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
        if (Input.GetMouseButtonDown(0))
        {
            timeSinceLastAttack = 0;
            // play animation
            Debug.Log("Attacking"); 
            weapon.GetComponent<Animator>().SetBool("Attacking", true);
            attacking = true;
        } else if (Input.GetMouseButtonUp(0))
        {
            weapon.GetComponent<Animator>().SetBool("Attacking", false);
            attacking = false;
        }
        timeSinceLastAttack += Time.deltaTime;

        if(Time.timeScale != 1 && timeSinceFreeze < 0.05f) {
            timeSinceFreeze += Time.unscaledDeltaTime;
        } else if(Time.timeScale != 1) {
            Time.timeScale = 1;
        }
    }

    public void KilledEnemy(int type) {
        money += type * 10;
        uimanager.UpdateMoney(money);
    }

    public float GetDamage() {
        Time.timeScale = 0.05f;
        timeSinceFreeze = 0;

        // Damage Particle Effect
        GameObject fx = Instantiate(hitFX, weapon.transform.position, Quaternion.identity);
        Destroy(fx, 1.0f);

        return realStats.baseDamage * realStats.attackMultiplier * (Velocity.magnitude / realStats.speed);
    }

    void UpdateStats() {
        // reset to base stats
        realStats = new Stats(baseStats);
        // apply item effects
        foreach (Item item in inventory) {
            for(int i = item.count; i > 0; i--) {
                Debug.Log("Applying item: " + item.itemname);
                Debug.Log("to player: " + this);
                item.UpdateStats(this);
            }
        }
    }

    public void GetCommonItem() {
        int item = UnityEngine.Random.Range(0, NUM_COMMON_ITEMS-1);
        inventory[item].count+=1;
        inventory[item].MakePopup();
        UpdateStats();
    }
    public void GetRareItem() {
        int item = UnityEngine.Random.Range(NUM_COMMON_ITEMS, NUM_RARE_ITEMS-1);
        inventory[item].count+=1;
        inventory[item].MakePopup();
        UpdateStats();
    }
    public void GetLegendaryItem() {
        int item = UnityEngine.Random.Range(NUM_COMMON_ITEMS+NUM_RARE_ITEMS, NUM_LEGENDARY_ITEMS-1);
        inventory[item].count+=1;
        inventory[item].MakePopup();
        UpdateStats();
    }
}
