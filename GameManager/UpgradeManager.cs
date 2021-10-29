using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField]
    private List<Material> upgradeMaterials = new List<Material>();

    [SerializeField]
    private int maxNumberOfUpgrades;
    private int upgradeMaterialId;
    private CastleHealth castleHealth;

    [SerializeField]
    private Costs upgradeCosts;

    private ButtonManager buttonManager;

    private SuppliesManager suppliesManager;

    private WallBuilding wallBuilding;

    [SerializeField]
    private TMP_Text starText;

    void Start()
    {
        castleHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<CastleHealth>();
        buttonManager = GetComponent<ButtonManager>();
        suppliesManager = GetComponent<SuppliesManager>();
        buttonManager.upgradeEvent.AddListener(PerformUpgrade);
        upgradeMaterialId = 0;
        wallBuilding = GetComponent<WallBuilding>();
    }

    void Update()
    {

    }

    void PerformUpgrade()
    {
        List<GameObject> walls = castleHealth.walls;
        foreach (var w in walls)
        {
            WallHealth wallHealth = w.GetComponent<WallHealth>();
            if (wallHealth)
            {
                wallHealth.maxHealth *= 2;
                wallHealth.health = wallHealth.maxHealth;
            }
            w.GetComponent<Renderer>().material = upgradeMaterials[upgradeMaterialId];
        }

        wallBuilding.towerPrefab.GetComponent<Renderer>().material = upgradeMaterials[upgradeMaterialId];
        wallBuilding.wallPrefab.GetComponent<Renderer>().material = upgradeMaterials[upgradeMaterialId];
        WallHealth towerPrefabHealth = wallBuilding.towerPrefab.GetComponent<WallHealth>();
        WallHealth wallPrefabHealth = wallBuilding.wallPrefab.GetComponent<WallHealth>();
        towerPrefabHealth.maxHealth *= 2;
        towerPrefabHealth.health = towerPrefabHealth.maxHealth;
        wallPrefabHealth.wallPartHealth *= 2;

        upgradeMaterialId++;
        if (upgradeMaterialId == maxNumberOfUpgrades)
        {
            suppliesManager.PerformTransaction("Upgrade");
            buttonManager.DisableStarButton();
            upgradeMaterialId -= 1;
            starText.text = "<b>-<b>     <sprite=0>";
        }
        else
        {
            suppliesManager.PerformTransaction("Upgrade");
            upgradeCosts.gold *= 2;
            starText.text = upgradeCosts.gold.ToString() + " <sprite=0>";
        }
    }
}
