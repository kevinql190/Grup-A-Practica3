using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn", menuName = "Dialogue/EndNodes/Spawn", order = 1)]
public class EndNode_Spawn : EndNode
{
    public override void OnChosen(GameObject talker)
    {
        base.OnChosen(talker);
        talker.GetComponent<Actions>().Spawn();
    }
}