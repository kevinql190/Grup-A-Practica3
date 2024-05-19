using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirstPhase : BossFightBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _bossFight.firstWave.enabled = true;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (_bossFight.firstWave.waveEnded)
        {
            animator.SetBool("FinishWave1", true);
            _bossFight.StartCameraSequence(4f);
        }
    }
}
