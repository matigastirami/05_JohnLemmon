using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WaypointPatrol : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform[] wayPoints;

    private int currentWayPointIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.SetDestination(wayPoints[currentWayPointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
        {
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length;
            _navMeshAgent.SetDestination(wayPoints[currentWayPointIndex].position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        // Print Each Ghost path
        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
        }
        
        Gizmos.DrawLine(wayPoints[0].position, wayPoints[wayPoints.Length - 1].position);
        
    }
}
