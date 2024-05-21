using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoNothing", menuName = "Dialogue/EndNodes/DoNothing", order = 1)]
public class EndNode : DialogueNode
{
    public virtual void OnChosen(GameObject talker) 
    {

    }
}
