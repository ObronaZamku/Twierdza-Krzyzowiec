using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class AIMovement : MonoBehaviour
{
    [SerializeField]
    private AIStats stats;

    [SerializeField]
    private float cooldownTime;

    private Vector3 destination;

    private GameObject focusedObject;

    private NavMeshAgent navMeshAgent;

    private float defaultSpeed;

    private float timer;

    bool obstacleOnTheWay = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        destination = GameObject.FindGameObjectWithTag("Player").transform.position;
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(destination, path);
        navMeshAgent.SetPath(path);
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.CalculatePath(destination, new NavMeshPath()) && navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            if (!navMeshAgent.pathPending)
            {
                float distance = 0.0f;
                Vector3[] corners = navMeshAgent.path.corners;
                for (int c = 0; c < corners.Length - 1; ++c)
                {
                    distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
                }
                if (distance <= 15f)
                {
                    if (navMeshAgent.destination != destination)
                    {
                        navMeshAgent.SetDestination(destination);
                    }
                }
            }
        }
        else
        {
            if (!obstacleOnTheWay)
            {
                focusedObject = GameObject.FindGameObjectWithTag("Wall");
                if (focusedObject != null)
                {
                    navMeshAgent.SetDestination(focusedObject.transform.position);
                    navMeshAgent.stoppingDistance = 3f;
                    obstacleOnTheWay = true;
                }
            }
        }

        timer += Time.deltaTime;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Tower")
        {
            if (timer >= cooldownTime)
            {
                timer = 0.0f;
                WallHealth wallHealth = other.GetComponent<WallHealth>();
                wallHealth.ChangeHealth(stats.attack);
                if (wallHealth.isDestroyed())
                {
                    obstacleOnTheWay = false;
                }
            }
        }
        else if (other.gameObject.tag == "Player")
        {
            if (timer >= cooldownTime)
            {
                timer = 0.0f;
                CastleHealth castleHealth = other.GetComponent<CastleHealth>();
                castleHealth.ChangeHealth(stats.attack);
            }
        }
    }

}
