using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBehaviour : StateMachineBehaviour
{
    protected Transform _player;
    public Enemy enemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.gameObject.GetComponentInParent<Enemy>(); 
    }
    // inRange -> Chase
    protected bool InRange(Transform mySelf, float enemyDetectionDistance)
    {
        float distance = Vector3.Distance(_player.position, mySelf.position);
        return distance < enemyDetectionDistance;
    }
    protected bool CheckPlayerVisibility(Animator animator)
    {
        Vector3 direction = (_player.position - animator.transform.position).normalized;
        if (Physics.Raycast(animator.transform.position, direction, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Player","Environment non walkable")))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
    // inRange -> Attack
    protected bool InRangeAttack(Transform mySelf, float attackRange)
    {
        float distance = Vector3.Distance(_player.position, mySelf.position);
        return distance < attackRange;
    }
}
