using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WallBuilding : MonoBehaviour
{
    [Header("Walls")]
    public GameObject wallPrefab;
    public GameObject towerPrefab;
    [SerializeField]
    private GameObject previewPrefab;
    [SerializeField]
    private GameObject wallPreview;


    [SerializeField]
    private Material forbiddenBuildMaterial;
    private Material previewMaterial;

    [SerializeField]
    private Material startMaterial;

    [SerializeField]
    private float maxDistance;

    private float distance;

    [Header("Camera")]
    [SerializeField]
    private Camera camera;

    private GameObject wallStart;
    private GameObject wallEnd;
    private GameObject wall;

    private bool buildingState;
    private bool canBuild = true;
    private GamePhases phases;
    private Vector3 startTowerPosition;
    private Vector3 startTowerForward;
    private SuppliesManager suppliesManager;

    private CastleHealth castleHealth;

    // Start is called before the first frame update
    void Start()
    {
        phases = GetComponent<GamePhases>();
        suppliesManager = GetComponent<SuppliesManager>();
        castleHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<CastleHealth>();
        previewMaterial = wallPreview.GetComponent<Renderer>().sharedMaterial;

        wallPrefab.GetComponent<Renderer>().material = startMaterial;
        towerPrefab.GetComponent<Renderer>().material = startMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (phases.isBuildingPhase && ButtonManager.currentMode == Modes.WALL)
        {
            WallPlacement();
        }
    }

    #region PrivateMethods

    void WallPlacement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouseButtonDown();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnRightMouseButtonDown();
        }
        else
        {
            AdjustWall();
        }
    }

    void OnLeftMouseButtonDown()
    {
        if (buildingState)
        {
            EndBuilding();
            return;
        }

        GameObject obj = EventSystem.current.currentSelectedGameObject;

        if (obj != null && obj.CompareTag("Button"))
        {
            return;
        }
        StartBuilding();
    }

    void OnRightMouseButtonDown()
    {
        if (!buildingState)
        {
            return;
        }

        if (!wallStart.CompareTag("Tower"))
        {
            Destroy(wallStart);
        }

        Destroy(wall);
        Destroy(wallEnd);
        buildingState = false;
    }

    void StartBuilding()
    {
        buildingState = true;

        if (!IfTowerHit(ref wallStart))
        {
            Vector3 mousePosition = GetWorldPoint();
            TryToCreateNewTower(mousePosition);
        }

        if (buildingState)
        {
            wall = Instantiate(wallPreview, wallStart.transform.position, Quaternion.identity) as GameObject;
            wallEnd = Instantiate(previewPrefab, wallStart.transform.position, Quaternion.identity) as GameObject;
            startTowerPosition = wallStart.transform.position;
        }
    }

    void TryToCreateNewTower(Vector3 mousePosition)
    {
        if (mousePosition == Vector3.zero)
        {
            buildingState = false;
            return;
        }

        wallStart = Instantiate(previewPrefab, mousePosition, Quaternion.identity) as GameObject;
    }

    void AdjustWall()
    {
        if (!buildingState)
        {
            return;
        }

        startTowerForward = wallStart.transform.forward;
        Vector3 mousePosition = GetWorldPoint();

        if (mousePosition == Vector3.zero || (wallStart.transform.position.y + 2f < mousePosition.y))
        {
            return;
        }

        distance = Vector3.Distance(startTowerPosition, mousePosition);

        if (!suppliesManager.EnoughForWall(distance))
        {
            wall.GetComponent<Renderer>().material = forbiddenBuildMaterial;
            if (!wallStart.CompareTag("Tower"))
            {
                wallStart.GetComponent<Renderer>().material = forbiddenBuildMaterial;
            }
            wallEnd.GetComponent<Renderer>().material = forbiddenBuildMaterial;
            canBuild = false;
            return;
        }
        else
        {
            wall.GetComponent<Renderer>().material = previewMaterial;
            if (!wallStart.CompareTag("Tower"))
            {
                wallStart.GetComponent<Renderer>().material = previewMaterial;
            }
            wallEnd.GetComponent<Renderer>().material = previewMaterial;
            canBuild = true;
        }

        wallStart.transform.LookAt(mousePosition);

        if (distance > maxDistance)
        {
            wall.transform.position = startTowerPosition + maxDistance / 2 * startTowerForward;
            wallEnd.transform.position = startTowerPosition + maxDistance * startTowerForward;
            wall.transform.rotation = wallStart.transform.rotation;
            distance = maxDistance;
            return;
        }

        wallEnd.transform.position = mousePosition;
        wallEnd.transform.LookAt(wallStart.transform.position);
        wall.transform.position = startTowerPosition + distance / 2 * startTowerForward;
        wall.transform.rotation = wallStart.transform.rotation;
        wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance);
    }


    void EndBuilding()
    {
        if (canBuild)
        {
            buildingState = false;
            Transform startPreviewTrans = wallStart.transform;
            Transform wallPreviewTrans = wall.transform;
            Transform endPreviewTrans = wallEnd.transform;

            if (!wallStart.CompareTag("Tower"))
            {
                Destroy(wallStart);
                wallStart = Instantiate(towerPrefab, startPreviewTrans.position, Quaternion.identity) as GameObject;
                castleHealth.walls.Add(wallStart);
            }

            Destroy(wallEnd);

            if (!IfTowerHit(ref wallEnd))
            {
                wallEnd = Instantiate(towerPrefab, endPreviewTrans.position, Quaternion.identity) as GameObject;
                castleHealth.walls.Add(wallEnd);
            }
            Destroy(wall);
            wall = Instantiate(wallPrefab, wallPreviewTrans.position, wallPreviewTrans.rotation) as GameObject;
            wall.GetComponent<WallHealth>().SetHealthAndMaxHealth(distance);
            castleHealth.walls.Add(wall);
            wall.transform.localScale = wallPreviewTrans.localScale;
            suppliesManager.WallBuilt(distance);
        }
    }

    Vector3 GetWorldPoint()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
        {
            return Vector3.zero;
        }

        switch (hit.collider.tag)
        {
            case "Tower":
                return hit.transform.position;
            case "Button":
            case "Wall":
                return Vector3.zero;
            case "Cannon":
            case "Bowman":
            case "Barrel":
                GameObject parentObject = hit.collider.transform.parent.gameObject;
                return GetParentTowerPosition(parentObject);
            default:
                return hit.point;
        }
    }

    Vector3 GetParentTowerPosition(GameObject parentObject)
    {
        if (parentObject && parentObject.tag == "Tower")
        {
            return parentObject.transform.position;
        }

        return Vector3.zero;
    }

    bool IfTowerHit(ref GameObject tower)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Tower")
        {
            tower = hit.collider.gameObject;
            return true;
        }

        return false;
    }
    #endregion
}
