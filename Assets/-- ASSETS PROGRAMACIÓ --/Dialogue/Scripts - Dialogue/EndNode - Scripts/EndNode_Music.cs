using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Music", menuName = "Dialogue/EndNodes/Music", order = 1)]
public class EndNode_Music : EndNode
{
    public override void OnChosen(GameObject talker)
    {
        base.OnChosen(talker);
        talker.GetComponent<Actions>().SoundMusic();
    }
}
