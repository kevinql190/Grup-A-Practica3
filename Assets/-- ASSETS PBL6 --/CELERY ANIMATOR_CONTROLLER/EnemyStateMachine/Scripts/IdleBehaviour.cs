using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : BaseBehaviour
{
    protected float _timer;
    public float WaitTime = 2;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _timer = 0;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // isChasing
        float enemyDetectionDistance = enemy.detectionDistance;
        if (InRange(animator.transform, enemyDetectionDistance) && CheckPlayerVisibility(animator)) 
        {
            animator.SetBool("isChasing", true);
        }
    }
    protected bool CheckTime()
    {
        _timer += Time.deltaTime;
        return _timer > WaitTime;
    }
}
