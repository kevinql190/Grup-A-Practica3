using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightBehaviour : StateMachineBehaviour
{
    protected Transform _player;
    protected FirstBossFightManager _bossFight;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _bossFight = animator.gameObject.GetComponent<FirstBossFightManager>();
    }
}
