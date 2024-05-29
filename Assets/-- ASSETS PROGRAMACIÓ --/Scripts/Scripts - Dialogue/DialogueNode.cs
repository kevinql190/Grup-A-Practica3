using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public string Text;
    public List<DialogueOption> Options;

    // EXTRA - Realizar acciones según la opción en medio del diálogo
    public Action<GameObject> NodeAction; 
}

[System.Serializable]
public class DialogueOption
{
    public string Text;
    public DialogueNode NextNode;
}
