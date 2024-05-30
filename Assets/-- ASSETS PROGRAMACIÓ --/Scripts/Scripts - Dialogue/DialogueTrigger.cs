using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Conversation Conversation;
    private bool isTriggerActive = true; 

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggerActive && other.tag == "Player")
        {
            DialogueManager.Instance.StartDialogue(Conversation, gameObject);
            Cursor.lockState = CursorLockMode.None;
            PlayerInputHandler.Instance.playerInput.SwitchCurrentActionMap("UI");
        }
    }

    // Método para desactivar completamente el trigger
    public void DisableTrigger()
    {
        isTriggerActive = false;
    }
}
