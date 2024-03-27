using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ITEM CONSTANTS
    int num_common_items;
    int num_rare_items;
    int num_legendary_items;
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

        num_common_items = 0;
        num_rare_items = 0;
        num_legendary_items = 0;
        foreach (Item item in inventory) {
            item.count = 0;
            if (item.rarity == 0) {
                num_common_items++;
            } else if (item.rarity == 1) {
                num_rare_items++;
            } else if (item.rarity == 2) {
                num_legendary_items++;
            }
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
        Cheats();
    }

    void Cheats() {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            GetCommonItem();
        if(Input.GetKeyDown(KeyCode.Alpha2))
            GetRareItem();
        if(Input.GetKeyDown(KeyCode.Alpha3))
            GetLegendaryItem();
    }

    void ApplyMovement() {
        Vector3 xzVelocity = new Vector3(Velocity.x, 0, Velocity.z);
        if (xzVelocity.magnitude > realStats.speed)
            Velocity += Acceleration / Mathf.Pow((xzVelocity.magnitude / realStats.speed),2);
        else
            Velocity += Acceleration;

        if(!handbraking) {
            // rotate velocity vector with rotation
            Vector3 xzVel = new Vector3(Velocity.x, 0, Velocity.z);
            Vector3 xzRot = Quaternion.Euler(0, Rotation*Time.deltaTime, 0) * xzVel;
            Velocity = new Vector3(xzRot.x, Velocity.y, xzRot.z);
        }
        Controller.Move(Velocity*Time.deltaTime);

        transform.Rotate(0, Rotation*(handbraking ? realStats.handbrakeMultiplier : 1)*Time.deltaTime, 0);
    }

    void DoMovement() {
        Vector3 dir = transform.forward;

        // handbrake keeps direction of movement constant while turning
        if (!handbraking || Velocity.magnitude < 0.1f) {
            hbForward = transform.forward;
        } else {
            // increment dir from transform.forward to hbForward
            dir = Vector3.RotateTowards(dir, hbForward, 0.01f, 0);
        }

        if(Input.GetKey(KeyCode.W) && !handbraking)
        {   
            Acceleration = transform.forward * realStats.speedMultiplier;
        } else if(Input.GetKey(KeyCode.S) && !handbraking)
        {
            Acceleration = -transform.forward * realStats.speedMultiplier;
        } else
        {
            Vector3 xzMovement = new Vector3(Velocity.x, 0, Velocity.z);
            if (Mathf.Abs(Velocity.magnitude) < realStats.speedMultiplier*2) {
                Velocity = new Vector3(0, Velocity.y, 0);
            } else if (!handbraking){
                // "friction"
                Acceleration = -xzMovement / 10f;
            } else {
                Acceleration = -xzMovement / (10f * realStats.handbrakeMultiplier);
            }
        }

        // slow rotation if moving quickly using magnitude of movement vector
        int sign = Vector3.Dot(Velocity, dir) > 0 ? 1 : -1;
        float rotFactor = sign * (Mathf.Abs(Velocity.magnitude) < 1 ? 0 : (1/Mathf.Sqrt(Mathf.Abs(Velocity.magnitude))));
        rotFactor = rotFactor * (handbraking ? realStats.handbrakeMultiplier : 1);
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

        float damage = realStats.baseDamage * realStats.attackMultiplier * (Velocity.magnitude / realStats.speed);

        // Damage Particle Effect
        GameObject fx = Instantiate(hitFX, weapon.transform.position, Quaternion.identity);
        // scale fx by damage 
        fx.transform.localScale = new Vector3(1, 1, 1) * damage/50;
        Destroy(fx, 1.0f);

        return damage;
    }

    void UpdateStats() {
        // reset to base stats
        realStats = new Stats(baseStats);
        // apply item effects
        foreach (Item item in inventory) {
            for(int i = item.count; i > 0; i--) {
                item.UpdateStats(this);
            }
        }
    }

    public void GetCommonItem() {
        int item = UnityEngine.Random.Range(0, num_common_items-1);
        inventory[item].count+=1;
        //inventory[item].MakePopup();
        UpdateStats();
    }
    public void GetRareItem() {
        int item = UnityEngine.Random.Range(num_common_items, num_common_items+num_rare_items-1);
        inventory[item].count+=1;
        //inventory[item].MakePopup();
        UpdateStats();
    }
    public void GetLegendaryItem() {
        int item = UnityEngine.Random.Range(num_common_items+num_rare_items, num_common_items+num_rare_items+num_legendary_items-1);
        Debug.Log("Item: " + item);
        inventory[item].count+=1;
        //inventory[item].MakePopup();
        UpdateStats();
    }
}
