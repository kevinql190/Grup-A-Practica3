using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actions : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private NavMeshObstacle navMeshObstacle;
    public Transform destinationObject;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    public void Move()
    {
        StartCoroutine(MoveAfterDelay(1.0f));
    }
    private IEnumerator MoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (navMeshObstacle != null)
        {
            navMeshObstacle.enabled = false;
        }

        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;

            if (destinationObject != null)
            {
                navMeshAgent.SetDestination(destinationObject.position);
            }
            else
            {
                Debug.LogWarning("Destination object is not set.");
            }
        }
    }
}
