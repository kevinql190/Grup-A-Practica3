using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomIdleBehaviour : BaseBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool("isChasing", false);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // isChasing
        float enemyDetectionDistance = enemy.attackRange;
        if (InRange(animator.transform, enemyDetectionDistance) && CheckPlayerVisibility(animator)) 
        {
            animator.SetBool("isChasing", true);
        }
    }
}
