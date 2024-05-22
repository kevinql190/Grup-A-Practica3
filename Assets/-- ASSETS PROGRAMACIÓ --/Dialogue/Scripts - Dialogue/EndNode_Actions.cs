using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicSad", menuName = "Dialogue/EndNodes/MusicSad", order = 1)]
public class EndNode_MusicSad : EndNode
{
    public override void OnChosen(GameObject talker)
    {
        base.OnChosen(talker);
       //talker.GetComponent<MovementController>.Flee();
    }
}

[CreateAssetMenu(fileName = "GoHome", menuName = "Dialogue/EndNodes/GoHome", order = 1)]
public class EndNode_GoHome : EndNode
{
    public override void OnChosen(GameObject talker)
    {
        base.OnChosen(talker);
        talker.GetComponent<Actions>().GoHome();
    }
}








