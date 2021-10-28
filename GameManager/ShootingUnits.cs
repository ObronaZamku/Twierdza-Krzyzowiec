using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ShootingUnits : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private Camera camera;

    [HideInInspector]
    public static Shootable chosenShootable;

    [SerializeField]
    private GameObject rangePreviewPrefab;
    [SerializeField]
    private Texture2D aimTexture;

    private GameObject rangePreview;
    private GameObject aimPreview;

    private Vector3 targetPosition;

    private float timer;
    private float reload;

    private GamePhases phases;

    GameObject[] enemies;
    GameObject[] bowmans;
    GameObject[] cannons;

    // Start is called before the first frame update
    void Start()
    {
        phases = GetComponent<GamePhases>();
        StartCoroutine(ShootingCoroutine("bowmans"));
        StartCoroutine(ShootingCoroutine("cannons"));
    }

    // Update is called once per frame
    void Update()
    {
        if (phases.isBuildingPhase)
        {
            return;
        }

        getObjectsForCoroutine();

        if (Input.GetMouseButtonDown(0))
        {
            Shootable clickedObject = GetClickedObjectIfShootable();
            if (clickedObject != null)
            {
                timer = 0;
                reload = clickedObject.GetReload();
                chosenShootable = clickedObject;
                MonoBehaviour go = clickedObject as MonoBehaviour;
                /*if(go != null){
                    rangePreview = Instantiate(rangePreviewPrefab, go.transform.position, Quaternion.identity) as GameObject;
                    rangePreview.transform.localScale = new Vector3(100f, 1000f, 100f);
                }*/
                Cursor.SetCursor(aimTexture, Vector2.zero, CursorMode.Auto);
                return;
            }
            if (clickedObject == null && chosenShootable != null)
            {
                if (timer > reload)
                {
                    MonoBehaviour chosenShootableGameObject = chosenShootable as MonoBehaviour;
                    Vector3 shootingUnitPosition = chosenShootableGameObject.transform.position;
                    targetPosition = GetWorldPositon();

                    Shoot(targetPosition, shootingUnitPosition, chosenShootable.GetAmmunition());
                    timer = 0;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Destroy(aimPreview);
            Destroy(rangePreview);
            chosenShootable = null;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        if (timer < reload)
        {
            timer += Time.deltaTime;
        }

    }

    private void getObjectsForCoroutine()
    {
        var randomNumber = new Random();
        enemies = GameObject.FindGameObjectsWithTag("AI");
        bowmans = GameObject.FindGameObjectsWithTag("Bowman");
        cannons = GameObject.FindGameObjectsWithTag("Cannon");
        Shuffle(randomNumber, enemies);
    }

    Shootable GetClickedObjectIfShootable()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
        {
            return null;
        }

        return hit.transform.gameObject.GetComponent<Shootable>();
    }

    Vector3 GetWorldPositon()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
        {
            return Vector3.zero;
        }

        return hit.point;
    }

    float GetXZDistance(Vector3 v1, Vector3 v2)
    {
        Vector3 temp1 = new Vector3(v1.x, 0f, v1.z);
        Vector3 temp2 = new Vector3(v2.x, 0f, v2.z);
        return Vector3.Distance(temp1, temp2);
    }

    IEnumerator ShootingCoroutine(String unitType)
    {
        float reloadTime = 2;
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
                    Shootable shootableUnit = shootingUnit.GetComponent<Shootable>();
                    reloadTime = shootableUnit.GetReload();
                    if (shootableUnit != chosenShootable)
                    {
                        HandleShooting(shootingUnit, shootableUnit);
                    }
                }
            }

            yield return new WaitForSeconds(reloadTime);
        }
    }

    private void HandleShooting(GameObject shootingUnit, Shootable shootableUnit)
    {
        Vector3 shootingUnitPosition = shootingUnit.transform.position;
        foreach (GameObject enemy in enemies)
        {
            Vector3 enemyPosition = enemy.transform.position;
            float distance = Vector3.Distance(shootingUnitPosition, enemyPosition);
            if (distance < shootableUnit.GetRange())
            {
                Shoot(enemyPosition, shootingUnitPosition, shootableUnit.GetAmmunition());
                return;
            }
        }
    }

    private void Shoot(Vector3 targetPosition, Vector3 shootingUnitPosition, GameObject ammunitionPrefab)
    {
        shootingUnitPosition.y += 3;
        Vector3 shootDirection = targetPosition - shootingUnitPosition;
        GameObject missile = Instantiate(ammunitionPrefab, shootingUnitPosition, Quaternion.identity) as GameObject;
        Rigidbody rigidbody = missile.GetComponent<Rigidbody>();
        rigidbody.AddForce(shootDirection, ForceMode.Impulse);
        Destroy(missile, 2f);
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
public interface Shootable
{
    float GetRange();
    float GetReload();
    GameObject GetAmmunition();
}
