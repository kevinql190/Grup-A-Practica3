using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSecondPhase : BossFightBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _bossFight.secondtWave.enabled = true;
        _bossFight.ThrowWing();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (_bossFight.secondtWave.waveEnded)
        {
            animator.SetBool("FinishWave2", true);
            _bossFight.StartCameraSequence(4f);
        }
    }
}
