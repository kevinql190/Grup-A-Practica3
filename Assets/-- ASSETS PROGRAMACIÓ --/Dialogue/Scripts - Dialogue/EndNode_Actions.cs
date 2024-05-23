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

[CreateAssetMenu(fileName = "Music", menuName = "Dialogue/EndNodes/Music", order = 1)]
public class EndNode_Music : EndNode
{
    public override void OnChosen(GameObject talker)
    {
        base.OnChosen(talker);
        talker.GetComponent<Actions>().PlayMusic();
    }
}

[CreateAssetMenu(fileName = "Attack", menuName = "Dialogue/EndNodes/Attack", order = 1)]
public class EndNode_Attack : EndNode
{
    public override void OnChosen(GameObject talker)
    {
        base.OnChosen(talker);
        talker.GetComponent<Actions>().Attack();
    }
}

[CreateAssetMenu(fileName = "Spawn", menuName = "Dialogue/EndNodes/Spawn", order = 1)]
public class EndNode_Spawn : EndNode
{
    public override void OnChosen(GameObject talker)
    {
        base.OnChosen(talker);
        talker.GetComponent<Actions>().Spawn();
    }
}








