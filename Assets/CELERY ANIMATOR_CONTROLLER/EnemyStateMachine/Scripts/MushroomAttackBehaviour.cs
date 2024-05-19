using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAttackBehaviour : BaseBehaviour
{
    private MushroomAttack mushroomAttack;
    private float waveDuration;
    protected float _timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        mushroomAttack = enemy.GetComponent<MushroomAttack>();
        _timer = 0;

        if (mushroomAttack != null)
        {
            waveDuration = mushroomAttack.waveDuration;
            enemy.Attack(); 
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mushroomAttack != null)
        {
            if (CheckTime())
            {
                animator.SetBool("isAttacking", false);
            }
        }
    }

    protected bool CheckTime()
    {
        _timer += Time.deltaTime;
        return _timer > waveDuration;
    }

}

