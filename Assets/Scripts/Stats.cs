using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public float maxHealth;
    public float speed;
    public float speedMultiplier;
    public float jumpForce;
    public float baseDamage;
    public float attackMultiplier;
    public float defenseMultiplier;
    public float attackSpeed;
    public float gravity;
    public float handbrakeMultiplier;
    public float knockback;
    public Stats(Dictionary<string, float> stats = null) {
        maxHealth = (stats != null && stats["health"] > 0) ? stats["health"] : 100;
        speed = (stats != null && stats["speed"] > 0) ? stats["speed"] : 20;
        speedMultiplier = (stats != null && stats["speedMultiplier"] > 0) ? stats["speedMultiplier"] : 0.5f;
        jumpForce = (stats != null && stats["jumpForce"] > 0) ? stats["jumpForce"] : 20;
        baseDamage = (stats != null && stats["baseDamage"] > 0) ? stats["baseDamage"] : 20;
        attackMultiplier = (stats != null && stats["attackMultiplier"] > 0) ? stats["attackMultiplier"] : 1;
        defenseMultiplier = (stats != null && stats["defenseMultiplier"] > 0) ? stats["defenseMultiplier"] : 1;
        attackSpeed = (stats != null && stats["attackSpeed"] > 0) ? stats["attackSpeed"] : 1;
        gravity = (stats != null && stats["gravity"] < 0) ? stats["gravity"] : -100f;
        handbrakeMultiplier = (stats != null && stats["handbrakeMultiplier"] > 0) ? stats["handbrakeMultiplier"] : 0;
        knockback = (stats != null && stats["knockback"] > 0) ? stats["knockback"] : 1;
    }

    public Stats(Stats other) {
        if (other != null) {
            maxHealth = other.maxHealth;
            speed = other.speed;
            speedMultiplier = other.speedMultiplier;
            jumpForce = other.jumpForce;
            baseDamage = other.baseDamage;
            attackMultiplier = other.attackMultiplier;
            defenseMultiplier = other.defenseMultiplier;
            attackSpeed = other.attackSpeed;
            gravity = other.gravity;
            handbrakeMultiplier = other.handbrakeMultiplier;
            knockback = other.knockback;
        }
    }
}