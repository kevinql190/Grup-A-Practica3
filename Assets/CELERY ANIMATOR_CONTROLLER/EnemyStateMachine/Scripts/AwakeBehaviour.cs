using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeBehaviour : BaseBehaviour
{
    protected float _timer;
    public float AwakeTime = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _timer = 0;
        animator.SetBool("isAwaked", false);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // isAwaked
        bool time = CheckTime();
        animator.SetBool("isAwaked", time);
    }
    protected bool CheckTime()
    {
        _timer += Time.deltaTime;
        return _timer > AwakeTime;
    }
}
