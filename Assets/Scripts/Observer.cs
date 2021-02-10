using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Observer : MonoBehaviour
{
    [SerializeField] private Transform observed;
    [SerializeField] private GameEnding gameEnding;

    private bool isPlayerInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == observed)
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == observed)
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            Vector3 direction = observed.position - transform.position + Vector3.up;

            Ray ray = new Ray(transform.position, direction);

            RaycastHit raycastHit;

            // out = pass the variable reference to get the collided object as output param
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == observed)
                {
                    gameEnding.CatchPlayer();
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, observed.position);   
    }
}
