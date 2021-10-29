using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RepairManager : MonoBehaviour
{

    private GameObject building;

    [SerializeField]
    private Text wallHPText;

    [SerializeField]
    private Text repairCostText;

    [Header("Camera")]
    [SerializeField]
    private Camera camera;

    private SuppliesManager suppliesManager;

    private GamePhases phases;

    void Start()
    {
        suppliesManager = GetComponent<SuppliesManager>();
        phases = GetComponent<GamePhases>();
    }

    // Update is called once per frame
    void Update()
    {
        if (phases.isBuildingPhase)
        {
            if (ButtonManager.currentMode == Modes.REPAIR)
            {
                ConstructionFound();
            }
            if (Input.GetMouseButtonDown(1))
            {
                ButtonManager.currentMode = Modes.DEFAULT;
                ButtonManager.ResetCursor();
            }
        }
    }

    void ConstructionFound()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Tower" || hit.collider.tag == "Wall"))
        {
            building = hit.collider.gameObject;
            WallHealth wallHealth = building.GetComponent<WallHealth>();
            wallHPText.text = "HP: " + (Math.Floor((double)((float)wallHealth.health / (float)wallHealth.maxHealth * 100))).ToString() + "%";
            repairCostText.text = "Repair cost: " + ((wallHealth.maxHealth - wallHealth.health) / 8).ToString() + " Stone "
             + ((wallHealth.maxHealth - wallHealth.health) / 8).ToString() + " Wood";
            if (Input.GetMouseButtonUp(0))
            {
                suppliesManager.ConstructionRepaired(wallHealth.maxHealth - wallHealth.health);
                wallHealth.health = wallHealth.maxHealth;
            }
        }
        else
        {
            wallHPText.text = "";
            repairCostText.text = "";
        }
    }


}
