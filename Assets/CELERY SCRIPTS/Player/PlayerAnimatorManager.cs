using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        GetComponent<PlayerMovement>().OnDashStart += ctx => animator.SetBool("dash", ctx);
    }
    private void Update()
    {
        animator.SetFloat("moveMagnitude", PlayerInputHandler.MoveInput.sqrMagnitude);
        bool isAttacking = animator.GetCurrentAnimatorStateInfo(1).IsName("Attack");
        animator.SetLayerWeight(1, isAttacking ? 1 : 0);
    }
    public void TriggerAnimation(string name)
    {
        animator.SetTrigger(name);
    }
}
