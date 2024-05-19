using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThirdPhase : BossFightBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _bossFight.thirdWave.enabled = true;
        _bossFight.ThrowHead();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (_bossFight.thirdWave.waveEnded)
        {
            animator.SetBool("FinishWave3", true);
            //_bossFight.StartCameraSequence(4f);
        }
    }
}
