using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{

    private Transform destination;


    private NavMeshAgent navMeshAgent;

    private float defaultSpeed;


    bool movementStarted = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        destination = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent.SetDestination(destination.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!movementStarted)
        {
            movementStarted = true;
            navMeshAgent.SetDestination(destination.position);
        }
        if (!navMeshAgent.hasPath)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("Wall");
            if (temp != null)
            {
                navMeshAgent.SetDestination(temp.transform.position);
            }
        }
    }



    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Wall" && other.gameObject.tag == "Tower")
        {
            navMeshAgent.velocity = new Vector3(0, 0, 0);
        }
    }
}
