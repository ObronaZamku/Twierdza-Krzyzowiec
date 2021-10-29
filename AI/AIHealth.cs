using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{

    public AIStats stats;

    public float currentHP;

    [SerializeField]
    private GameObject gameManager;

    private SuppliesManager suppliesManager;
    void Start()
    {
        currentHP = stats.health;
        suppliesManager = gameManager.GetComponent<SuppliesManager>();
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
            currentHP -= 40;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            currentHP -= 200;
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
            suppliesManager.GetReward(stats);
        }
    }


}
