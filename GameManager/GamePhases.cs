using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamePhases : MonoBehaviour
{

    [Header("Building phase timer")]
    [SerializeField]
    private float buildingPhaseTime;
    [SerializeField]
    private bool timedBuilding;

    private float timer = 0f;

    [SerializeField]
    private float spawnInterval;

    [SerializeField]
    private int spawnCount;

    private int spawns = 0;

    [HideInInspector]
    public bool isBuildingPhase = true;
    [HideInInspector]
    public int phaseCount = 0;

    private ButtonManager buttonManager;

    private List<EnemySpawn> enemySpawners;

    private bool ongoingSiege = false;

    private SuppliesManager suppliesManager;

    // Start is called before the first frame update
    void Start()
    {
        EnemySpawn[] es = GetComponentsInChildren<EnemySpawn>();
        enemySpawners = new List<EnemySpawn>(es);
        buttonManager = GetComponent<ButtonManager>();
        suppliesManager = GetComponent<SuppliesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuildingPhase)
        {
            if (timedBuilding)
            {
                Timer();
            }
        }
        else
        {
            if (!ongoingSiege)
            {
                Timer();
            }
            if (spawns != 0)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("AI");
                if (enemies.Length == 0)
                {
                    isBuildingPhase = true;
                    buttonManager.ButtonTurnOn();
                    ButtonManager.currentMode = Modes.DEFAULT;
                    ongoingSiege = false;
                    spawns = 0;
                    phaseCount++;
                    suppliesManager.PhaseReward(phaseCount);
                    ButtonManager.ResetCursor();
                }
            }
        }


        if (ButtonManager.currentMode == Modes.SIEGE && isBuildingPhase)
        {
            isBuildingPhase = !isBuildingPhase;
            timer = 0.0f;
        }
    }

    private void Timer()
    {
        if (isBuildingPhase)
        {
            timer += Time.deltaTime;
            if (timer > buildingPhaseTime)
            {
                isBuildingPhase = false;
                timer = 0.0f;
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > spawnInterval)
            {
                foreach (EnemySpawn es in enemySpawners)
                {
                    Dictionary<string, int> spawnMap = SpawnPrepare();
                    es.SpawnEnemies(spawnMap);
                }
                spawns++;
                timer = 0.0f;
                if (spawns == spawnCount)
                {
                    ongoingSiege = true;
                }
            }
        }
    }

    private Dictionary<string, int> SpawnPrepare()
    {
        Dictionary<string, int> spawnMap = new Dictionary<string, int>();
        spawnMap.Add("trooper", phaseCount * phaseCount + 2);
        spawnMap.Add("wizard", 2 * phaseCount + 1);
        spawnMap.Add("troll", phaseCount - 1);
        return spawnMap;
    }

}
