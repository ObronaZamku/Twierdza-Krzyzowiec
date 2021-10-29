using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : SavableWithHealth
{

    [SerializeField]
    public int wallPartHealth;

    [SerializeField]
    private int wallPartHealthStart;

    [SerializeField]
    private int towerMaxHealthStart;

    private CastleHealth castleHealth;
    private bool isStarted = false;

    void Start()
    {
        if (gameObject.tag == "Tower")
        {
            maxHealth = towerMaxHealthStart;
            health = maxHealth;
        }
        castleHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<CastleHealth>();
    }

    void Update()
    {
        if (isDestroyed())
        {
            castleHealth.walls.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void SetHealthAndMaxHealth(float distance)
    {
        if (!isStarted)
        {
            wallPartHealth = wallPartHealthStart;
            isStarted = true;
        }
        health = (int)distance * wallPartHealth;
        maxHealth = health;
    }


    public void ChangeHealth(int hitPoints)
    {
        health -= hitPoints;
    }

    public bool isDestroyed()
    {
        return health <= 0;
    }
}
