using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Animator dialogueAnimator;

    public static DialogueManager Instance;

    private DialogueNode _currentNode;

    private GameObject _talker;

    public TextMeshProUGUI NameText;
    public TextMeshProUGUI SpeechText;
    public TextMeshProUGUI[] OptionsText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    public void StartDialogue(Conversation conversation, GameObject talker)
    {
        _talker = talker;
        _currentNode = conversation.StartNode;
        NameText.text = conversation.Name;
        SetNode(_currentNode);
        ShowDialogue();
    }

    private void SetNode(DialogueNode currentNode)
    {
        SpeechText.text = currentNode.Text;
        for (int i = 0; i < OptionsText.Length; i++)
        {
            if (i < currentNode.Options.Count)
            {
                OptionsText[i].transform.parent.gameObject.SetActive(true);
                OptionsText[i].text = currentNode.Options[i].Text;
            }
            else
            {
                OptionsText[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void ShowDialogue()
    {
        dialogueAnimator.SetBool("Show", true);
    }
    public void HideDialogue()
    {
        dialogueAnimator.SetBool("Show", false);
    }

    //------- Cambio de opción ------
    private void SetText(DialogueNode node) 
    {
        SpeechText.text = node.Text;
        for (int i = 0; i < OptionsText.Length; i++)
        {
            if (i < node.Options.Count)
            {
                OptionsText[i].transform.parent.gameObject.SetActive(true);
                OptionsText[i].text = node.Options[i].Text;
            }
            else 
            {
                OptionsText[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void OptionChosen(int option)
    {
        if (option >= 0 && option < _currentNode.Options.Count)
        {
            _currentNode = _currentNode.Options[option].NextNode;
            SetText(_currentNode);
            // Ejecutar la acción del nodo si existe
            _currentNode.NodeAction?.Invoke(_talker);

            if (_currentNode is EndNode)
            {
                DoEndNode(_currentNode as EndNode);
            }
            else
            {
                SetText(_currentNode);
            }
        }
    }

    private void DoEndNode(EndNode currentNode)
    {
        currentNode.OnChosen(_talker);
        StartCoroutine(HideDialogueWithDelay());
    }

    private IEnumerator HideDialogueWithDelay()
    {
        yield return new WaitForSeconds(1f); // Espera 1 segundo
        HideDialogue();
    }
}