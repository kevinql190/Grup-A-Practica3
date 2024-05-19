using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent enemy;
    private Transform playerTarget;
    [Header("Movement Radius")]
    public float detectionDistance = 10f;

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        playerTarget = GameObject.FindGameObjectsWithTag("Player")[0].transform; 
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        //Debug.Log("Distancia al jugador: " + distanceToPlayer);
        enemy.SetDestination(playerTarget.position);
        if (distanceToPlayer <= detectionDistance)
        {
            enemy.SetDestination(playerTarget.position);
        }
        else
        {
            enemy.ResetPath();
        }
    }
}
