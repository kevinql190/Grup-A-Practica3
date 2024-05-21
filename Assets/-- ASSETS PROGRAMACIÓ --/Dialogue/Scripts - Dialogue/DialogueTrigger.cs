using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Conversation Conversation;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Trigger Enter");
            DialogueManager.Instance.StartDialogue(Conversation, gameObject);
            Cursor.lockState = CursorLockMode.None;
            PlayerInputHandler.Instance.playerInput.SwitchCurrentActionMap("UI");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerInputHandler.Instance.playerInput.SwitchCurrentActionMap("Gameplay");
    }
}
