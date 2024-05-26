using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Dialogue/EndNodes/Move", order = 1)]
public class EndNode_Move : EndNode
{
    public override void OnChosen(GameObject talker)
    {
        base.OnChosen(talker);
        talker.GetComponent<Actions>().Move();
    }
}

[CreateAssetMenu(fileName = "GoHome", menuName = "Dialogue/EndNodes/GoHome", order = 1)]
public class EndNode_GoHome : EndNode
{
    public override void OnChosen(GameObject talker)
    {
        base.OnChosen(talker);
       // talker.GetComponent<Actions>().GoHome();
    }
}








