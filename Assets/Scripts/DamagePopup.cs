using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    Text text;
    int damage;
    float timer = 0;

    void Start() {
        GetComponent<Canvas>().worldCamera = Camera.main;
        text = GetComponent<Text>();
    }
    void Update() {
        text.color = new Color(Mathf.Sin(Mathf.PI * (damage / 100)), 1, Mathf.Cos(Mathf.PI * (damage / 100)), 1);
        text.fontSize = 20;
        text.text = damage.ToString();

        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        transform.position += new Vector3(0, 0.01f, 0);

        timer += Time.deltaTime;
        if (timer > 1) {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage) {
        this.damage = (int)damage;
    }
}
