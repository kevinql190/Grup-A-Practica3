using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flee", menuName = "Dialogue/EndNodes/Flee", order = 1)]
public class EndNode_Flee : EndNode
{
    public override void OnChosen(GameObject talker)
    {
        base.OnChosen(talker);
       // talker.GetComponent<MovementController>.Flee();
    }
}

