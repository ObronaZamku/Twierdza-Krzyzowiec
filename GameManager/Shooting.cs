using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    [Header("Camera")]
    [SerializeField]
    private Camera camera;

    private GamePhases phases;

    [HideInInspector]
    public static Shootable chosenShootable;

    [SerializeField]
    private GameObject rangePreviewPrefab;
    [SerializeField]
    private GameObject aimPreviewPrefab;

    private GameObject rangePreview;
    private GameObject aimPreview;

    private Vector3 targetPosition;

    private float timer;
    private float reload;
    
    void Start()
    {
        phases = GetComponent<GamePhases>();
    }

    void Update()
    {
        if(!phases.isBuildingPhase) {
            if(Input.GetMouseButtonDown(0)) {
                Shootable clickedObject = GetClickedObjectIfShootable();
                if(clickedObject != null) {
                    timer = 0;
                    reload = clickedObject.GetReload();
                    chosenShootable = clickedObject;
                    MonoBehaviour go = clickedObject as MonoBehaviour;
                    /*if(go != null){
                        rangePreview = Instantiate(rangePreviewPrefab, go.transform.position, Quaternion.identity) as GameObject;
                        rangePreview.transform.localScale = new Vector3(100f, 1000f, 100f);
                    }*/
                    aimPreview = Instantiate(aimPreviewPrefab) as GameObject;
                    return;
                }
                if(clickedObject == null && chosenShootable != null) {
                    if(timer > reload){
                        chosenShootable.Shoot(targetPosition);
                        timer = 0;
                    }
                }
            } else if(Input.GetMouseButtonDown(1)) {
                Destroy(aimPreview);
                Destroy(rangePreview);
                chosenShootable = null;
            }
            if(chosenShootable != null && aimPreview != null) {
                targetPosition = GetWorldPositon();
                if(!(Vector3.Distance(targetPosition, (chosenShootable as MonoBehaviour).transform.position) > chosenShootable.GetRange())){
                    aimPreview.transform.position = targetPosition;
                }
            }
            if(timer < reload){
                timer += Time.deltaTime;
            }
        }
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

    Vector3 GetWorldPositon() {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
        {
            return Vector3.zero;
        }

        return hit.point;
    }

    float GetXZDistance(Vector3 v1, Vector3 v2){
        Vector3 temp1 = new Vector3(v1.x, 0f, v1.z); 
        Vector3 temp2 = new Vector3(v2.x, 0f, v2.z);
        return Vector3.Distance(temp1, temp2);
    }
}

public interface Shootable {
    void Shoot(Vector3 targetPosition);
    float GetRange();
    float GetReload();
}
