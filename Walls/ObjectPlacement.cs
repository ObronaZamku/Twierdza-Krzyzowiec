using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [Header("Troops")]
    [SerializeField]
    private GameObject cannon;
    [SerializeField]
    private GameObject bowman;
    [SerializeField]
    private GameObject barrel;
    [Header("Camera")]
    [SerializeField]
    private Camera camera;

    private GamePhases phases;
    private SuppliesManager suppliesManager;


    // Start is called before the first frame update
    void Start()
    {
        phases = GetComponent<GamePhases>();
        suppliesManager = GetComponent<SuppliesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (phases.isBuildingPhase)
        {
            if (Input.GetMouseButtonDown(0) && ButtonManager.currentMode == Modes.CANNON)
            {
                instantiateTroop(cannon);
                suppliesManager.ObjectPlaced(cannon.tag);
                ButtonManager.currentMode = Modes.DEFAULT;
            }
            else if (Input.GetMouseButtonDown(0) && ButtonManager.currentMode == Modes.BOWMAN)
            {
                instantiateTroop(bowman);
                suppliesManager.ObjectPlaced(bowman.tag);
                ButtonManager.currentMode = Modes.DEFAULT;
            }
            else if (Input.GetMouseButtonDown(0) && ButtonManager.currentMode == Modes.BARREL)
            {
                instantiateTroop(barrel);
                suppliesManager.ObjectPlaced(barrel.tag);
                ButtonManager.currentMode = Modes.DEFAULT;
            }
        }
    }

    void instantiateTroop(GameObject prefab)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Tower")
            {
                GameObject trans = Instantiate(prefab, hit.point, Quaternion.identity) as GameObject;
                trans.transform.SetParent(hit.transform, false);
                trans.transform.localScale = Vector3.Scale(trans.transform.localScale, new Vector3(1 / hit.transform.localScale.x, 1 / hit.transform.localScale.y, 1 / hit.transform.localScale.z));
                trans.transform.position = new Vector3(hit.point.x, trans.GetComponent<Collider>().bounds.size.y / 2 + hit.point.y, hit.point.z);
            }
        }
    }
}