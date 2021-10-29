using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class CastleHealth : MonoBehaviour
{
    [SerializeField]
    int castleHealth;

    int maxCastleHealth;

    int currentMaxHealth;

    int currentMaxWallHealth;

    int health;

    public List<GameObject> walls = new List<GameObject>();

    public UnityEvent castleDestroyedEvent;

    [SerializeField]
    private Text healthText;

    void Start()
    {
        maxCastleHealth = castleHealth;
        currentMaxHealth = maxCastleHealth;
        health = castleHealth;
    }

    // Update is called once per frame
    void Update()
    {
        RecalculateHealth();
        healthText.text = "HP: " + (Math.Floor((double)((float)health / (float)currentMaxHealth * 100))).ToString() + "%";
    }


    void RecalculateHealth()
    {
        if (!isDestroyed())
        {
            health = castleHealth;
            int maxWallhealth = 0;
            foreach (var w in walls)
            {
                WallHealth wallHealth = w.GetComponent<WallHealth>();
                if (wallHealth)
                {
                    health += wallHealth.health;
                    maxWallhealth += wallHealth.maxHealth;
                }
            }
            if (maxWallhealth > currentMaxWallHealth)
            {
                currentMaxWallHealth = maxWallhealth;
                currentMaxHealth = maxCastleHealth + currentMaxWallHealth;
            }
        }
        else
        {
            Destroy(gameObject);
            health = 0;
            castleDestroyedEvent.Invoke();
        }
    }

    public void ChangeHealth(int hitPoints)
    {
        castleHealth -= hitPoints;
    }

    bool isDestroyed()
    {
        return castleHealth <= 0;
    }
}
