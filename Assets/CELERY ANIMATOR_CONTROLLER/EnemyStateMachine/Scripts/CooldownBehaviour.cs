using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBehaviour : BaseBehaviour
{
    protected float _timer;
    protected float cooldownAttack;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _timer = 0;
        animator.SetBool("postCooldown", false);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float attackRange = enemy.attackRange;
        cooldownAttack = enemy.cooldownAttack;
        enemy.FacePlayer();
        if (CheckTime()) 
        {
            animator.SetBool("postCooldown", true);
            animator.SetBool("inRangeAttack", InRange(animator.transform, attackRange));
        }
    }
    protected bool CheckTime()
    {
        _timer += Time.deltaTime;
        return _timer > cooldownAttack;
    }
}
