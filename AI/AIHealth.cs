using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{

    public AIStats stats;

    public float currentHP;
    void Start()
    {
        currentHP = stats.health;
    }

    // Update is called once per frame
    void Update()
    {
        CheckKill();
    }

    void CheckKill()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }


}
