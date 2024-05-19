using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MushroomPrepareAttackBehaviour : BaseBehaviour
{
    protected float _timer;
    protected float prepareAttackTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _timer = 0;
        enemy.FacePlayer();
        animator.SetBool("isAttacking", false);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        prepareAttackTime = enemy.prepareAttackTime;
        animator.SetBool("isAttacking", CheckTime());
    }
    protected bool CheckTime()
    {
        _timer += Time.deltaTime;
        return _timer > prepareAttackTime;
    }
}