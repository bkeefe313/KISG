using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public PlayerInventory playerInventory;
    public bool paused = false;
    public Sprite dmgFX;
    public float damageFxLevel = 0.0f;
    public float damageDuration = 1.0f;
    public bool bossActive = false;
    public EnemyManager currentBoss;
    public Image hpBarBackground; // Add your background image here(inside Unity)
    public Image hpBarForeground; // add the HP image that will be affected by Damage here
    public Canvas canvas;
    public GameObject portal;

    void Start()
    {
        canvas = GameObject.Find("hpCanvas").GetComponent<Canvas>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
        hpBarBackground = canvas.gameObject.transform.GetChild(0).GetComponent<Image>();
        hpBarForeground = canvas.gameObject.transform.GetChild(1).GetComponent<Image>();
        hpBarBackground.enabled = false;
        hpBarForeground.enabled = false;
    }
    void OnGUI()
    {
        GUI.Box(new Rect(40, 5, 1000, 90), "Player Stats");
        GUI.Label(new Rect(50, 20, 900, 20), "Health: " + playerManager.health + "/" + playerManager.realStats.maxHealth);
        GUI.Label(new Rect(50, 40, 900, 20), "Money: " + playerInventory.money);
        GUI.Label(new Rect(50, 60, 900, 20), "Inventory: " + inventoryString(playerInventory.inventory));
        if(paused)
        {
            // draw pause menu
            GUI.Box(new Rect(500, 200, 200, 100), "Pause Menu");
            if (GUI.Button(new Rect(550, 220, 100, 20), "Resume"))
            {
                TogglePauseMenu();
            }
            if (GUI.Button(new Rect(550, 240, 100, 20), "Quit"))
            {
                Application.Quit();
            }
        }
        if(damageFxLevel > 0)
        {
            Debug.Log("Drawing damage fx");
            damageFxLevel -= Time.deltaTime;
            // change opacity based on damageFxLevel
            Color color = GUI.color;
            color.a = damageFxLevel / damageDuration;
            GUI.color = color;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), dmgFX.texture);
        }
        if(bossActive)
        {
            hpBarBackground.enabled = true;
            hpBarForeground.enabled = true;
            hpBarForeground.fillAmount = currentBoss.health / currentBoss.maxHealth;
        } else {
            hpBarBackground.enabled = false;
            hpBarForeground.enabled = false;
        }
    }

    void Update()
    {
        if(bossActive) {
            gameObject.GetComponent<AudioSource>().mute = true;
        } else {
            gameObject.GetComponent<AudioSource>().mute = false;
        }
    }
    string inventoryString(List<Item> inventory)
    {
        string result = "";
        foreach (Item item in inventory)
        {
            if(item.count > 0)
                result += item.itemname + " x" + item.count + ", ";
        }
        return result;
    }

    public void UpdateMoney(int amount)
    {
        playerInventory.money = amount;
    }

    public void TogglePauseMenu()
    {
        Debug.Log("Toggling pause menu");
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        Cursor.visible = paused;
    }

    public void ActivateDamageFX() {
        damageFxLevel = damageDuration;
    }

    public void ActivateBoss(EnemyManager boss) {
        bossActive = true;
        currentBoss = boss;
    }

    public void BossDefeated() {
        // spawn portal
        Instantiate(portal, currentBoss.transform.position, Quaternion.identity);

        bossActive = false;
        currentBoss = null;
    }
}
