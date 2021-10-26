using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    [SerializeField]
    private GameObject trooper;
    [SerializeField]
    private GameObject wizard;
    [SerializeField]
    private GameObject troll;

    public bool debugTurnOff;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnEnemies(Dictionary<string, int> spawnMap)
    {
        foreach (KeyValuePair<string, int> p in spawnMap)
        {
            switch (p.Key)
            {
                case "trooper":
                    InstantiateNTimes(p.Value, trooper);
                    break;

                case "wizard":
                    InstantiateNTimes(p.Value, wizard);
                    break;

                case "troll":
                    InstantiateNTimes(p.Value, troll);
                    break;
            }
        }
    }

    void InstantiateNTimes(int n, GameObject prefab)
    {
        if (!debugTurnOff)
        {
            for (int i = 0; i < n; i++)
            {
                Instantiate(prefab, transform.position + new Vector3(Random.Range(-15f, 15f), 0, Random.Range(-15f, 15f)), Quaternion.identity);
            }
        }
    }

}
