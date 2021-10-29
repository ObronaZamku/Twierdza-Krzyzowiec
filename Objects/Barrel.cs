using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [Header("Oil")]
    [SerializeField]
    private GameObject oil;

    [Header("Camera")]
    [SerializeField]
    private Camera camera;

    private GamePhases phases;

    void Start()
    {
        phases = GetComponent<GamePhases>();
    }

    void Update()
    {
        if (!phases.isBuildingPhase && Input.GetMouseButtonDown(0))
        {
            OilSpill();
        }
    }

    void OilSpill()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Barrel")
            {
                Destroy(hit.collider.gameObject);
                for (int i = 0; i < 50; i++)
                {
                    GameObject trans = Instantiate(oil, hit.point, Quaternion.identity) as GameObject;
                    Rigidbody rigidbody = trans.GetComponent<Rigidbody>();
                    rigidbody.velocity = new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-50f, 50f));
                    //rigidbody.AddForce(trans.transform.forward * Random.Range(-10, 10));
                    Destroy(trans, 10f);
                }
            }
        }
    }


}
