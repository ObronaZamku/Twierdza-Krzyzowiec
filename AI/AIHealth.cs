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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            currentHP -= 10;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            currentHP -= 40;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Oil"))
        {
            currentHP -= 20;
            Destroy(other.gameObject);
        }
    }

    void CheckKill()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }


}
