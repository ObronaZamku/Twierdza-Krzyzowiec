using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ShootingUnits : MonoBehaviour
{
    [Header("Range")]
    [SerializeField]
    private float bowmanRange;
    [SerializeField]
    private float cannonRange;

    [Header("Reload time")]
    [SerializeField]
    private int bowmanReloadTime;
    [SerializeField]
    private int cannonReloadTime;

    [Header("Ammunition")]
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private GameObject bulletPrefab;

    private GamePhases phases;

    GameObject[] enemies;
    GameObject[] bowmans;
    GameObject[] cannons;

    // Start is called before the first frame update
    void Start()
    {
        phases = GetComponent<GamePhases>();
        StartCoroutine(Shoot("bowmans", arrowPrefab, bowmanReloadTime, bowmanRange));
        StartCoroutine(Shoot("cannons", bulletPrefab, cannonReloadTime, cannonRange));
    }

    // Update is called once per frame
    void Update()
    {
        if (!phases.isBuildingPhase)
        {
            var randomNumber = new Random();
            enemies = GameObject.FindGameObjectsWithTag("AI");
            bowmans = GameObject.FindGameObjectsWithTag("Bowman");
            cannons = GameObject.FindGameObjectsWithTag("Cannon");
            Shuffle(randomNumber, enemies);
            Shuffle(randomNumber, bowmans);
            Shuffle(randomNumber, cannons);
        }
    }

    IEnumerator Shoot(String unitType, GameObject missilePrefab, int reloadTime, float shootingRange)
    {
        while (true)
        {
            if (!phases.isBuildingPhase)
            {
                GameObject[] shootingUnits;
                if (unitType == "bowmans")
                {
                    shootingUnits = bowmans;
                }
                else
                {
                    shootingUnits = cannons;
                }

                foreach (GameObject shootingUnit in shootingUnits)
                {
                    HandleShooting(shootingUnit, missilePrefab, shootingRange);
                }
            }

            yield return new WaitForSeconds(reloadTime);

        }
    }

    private void HandleShooting(GameObject shootingUnit, GameObject missilePrefab, float shootingRange)
    {
        Vector3 shootingUnitPosition = shootingUnit.transform.position;
        foreach (GameObject enemy in enemies)
        {
            Vector3 enemyPosition = enemy.transform.position;
            float distance = Vector3.Distance(shootingUnitPosition, enemyPosition);

            if (distance < shootingRange)
            {
                Vector3 shootDirection = enemyPosition - shootingUnitPosition;
                GameObject missile = Instantiate(missilePrefab, shootingUnitPosition, Quaternion.identity) as GameObject;
                Rigidbody rigidbody = missile.GetComponent<Rigidbody>();
                rigidbody.AddForce(shootDirection, ForceMode.Impulse);
                Destroy(missile, 5f);
                return;
            }
        }
    }

    public static void Shuffle<T>(Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
