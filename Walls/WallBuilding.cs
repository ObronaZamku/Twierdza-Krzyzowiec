using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WallBuilding : MonoBehaviour
{
    [Header("Walls")]
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private GameObject previewPrefab;
    [SerializeField]
    private GameObject wallPreview;
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
    private bool isExisting = false;

    private GamePhases phases;


    private Vector3 tempPosition;
    private Vector3 tempForward;


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
        if (phases.isBuildingPhase && ButtonManager.currentMode == Modes.WALL)
        {
            WallPlacement();
        }
    }

    #region PrivateMethods

    void WallPlacement()
    {
        if (Input.GetMouseButtonDown(0) && !buildingState)
        {
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            if (obj != null)
            {
                if (obj.tag == "Button")
                {
                    return;
                }
            }
            StartBuilding();
        }
        else if (Input.GetMouseButtonDown(0) && buildingState)
        {
            EndBuilding();
        }
        else if (Input.GetMouseButtonDown(1) && buildingState)
        {
            if (wallStart.tag != "Tower")
            {
                Destroy(wallStart);
            }
            Destroy(wall);
            Destroy(wallEnd);
            buildingState = false;
        }
        else
        {
            if (buildingState)
            {
                AdjustWall();
            }
        }
    }

    void StartBuilding()
    {
        buildingState = true;
        if (!IfStartTowerExists())
        {
            Vector3 temp = GetWorldPoint();
            if (temp == Vector3.zero)
            {
                buildingState = false;
                return;
            }
            wallStart = Instantiate(previewPrefab, GetWorldPoint(), Quaternion.identity) as GameObject;
        }
        wall = Instantiate(wallPreview, wallStart.transform.position, Quaternion.identity) as GameObject;
        wallEnd = Instantiate(previewPrefab, wallStart.transform.position, Quaternion.identity) as GameObject;
        tempPosition = wallStart.transform.position;
    }


    void AdjustWall()
    {
        tempForward = wallStart.transform.forward;
        Vector3 temp = GetWorldPoint();
        if (wallStart.transform.position.y != temp.y)
        {
            return;
        }
        if (temp != Vector3.zero)
        {
            distance = Vector3.Distance(tempPosition, temp);
            wallStart.transform.LookAt(temp);
            if (distance > maxDistance)
            {
                wall.transform.position = tempPosition + maxDistance / 2 * tempForward;
                wallEnd.transform.position = tempPosition + maxDistance * tempForward;
                wall.transform.rotation = wallStart.transform.rotation;
                distance = maxDistance;
                return;
            }
            wallEnd.transform.position = temp;
            wallEnd.transform.LookAt(wallStart.transform.position);
            wall.transform.position = tempPosition + distance / 2 * tempForward;
            wall.transform.rotation = wallStart.transform.rotation;
            wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance);

        }
    }


    void EndBuilding()
    {
        buildingState = false;
        Transform startPreview = wallStart.transform;
        Transform wallPreviewTrans = wall.transform;
        Transform endPreview = wallEnd.transform;
        if (wallStart.tag != "Tower")
        {
            Destroy(wallStart);
            wallStart = Instantiate(towerPrefab, startPreview.position, Quaternion.identity) as GameObject;
        }
        Destroy(wallEnd);
        if (!IfEndTowerExists())
        {
            wallEnd = Instantiate(towerPrefab, endPreview.position, Quaternion.identity) as GameObject;
        }
        Destroy(wall);
        wall = Instantiate(wallPrefab, wallPreviewTrans.position, wallPreviewTrans.rotation) as GameObject;
        wall.transform.localScale = wallPreviewTrans.localScale;
        suppliesManager.WallBuilt(distance);

    }

    Vector3 GetWorldPoint()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Tower")
            {
                return hit.transform.position;
            }
            if (hit.collider.tag == "Wall")
            {
                return Vector3.zero;
            }
            if (hit.collider.tag == "Cannon" || hit.collider.tag == "Bowman" || hit.collider.tag == "Barrel")
            {
                GameObject tower = hit.collider.transform.parent.gameObject;
                if (!tower)
                {
                    return Vector3.zero;
                }
                if (tower.tag == "Tower")
                {
                    return tower.transform.position;
                }
                return Vector3.zero;
            }
            if (hit.collider.tag == "Button")
            {
                return Vector3.zero;
            }
            return hit.point;
        }
        return Vector3.zero;
    }

    bool IfStartTowerExists()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Tower")
        {
            wallStart = hit.collider.gameObject;
            return true;
        }
        return false;
    }

    bool IfEndTowerExists()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Tower")
        {
            wallEnd = hit.collider.gameObject;
            return true;
        }
        return false;
    }

    #endregion

}
