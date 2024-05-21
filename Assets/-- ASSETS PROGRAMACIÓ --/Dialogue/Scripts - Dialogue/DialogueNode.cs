using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public string Text;
    public List<DialogueOption> Options;
}

[System.Serializable]
public class DialogueOption
{
    public string Text;
    public DialogueNode NextNode;
}
