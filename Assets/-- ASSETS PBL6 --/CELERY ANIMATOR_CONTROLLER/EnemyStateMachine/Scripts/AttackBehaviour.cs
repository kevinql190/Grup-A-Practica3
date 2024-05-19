using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackBehaviour : BaseBehaviour
{
    private bool hasAttacked = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        hasAttacked = false;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasAttacked)
        {
            // Attack
            enemy.Attack();
            hasAttacked = true;
        }
        animator.SetBool("isAttacking", false);

    }
}
