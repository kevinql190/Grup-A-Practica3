using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossFightRoom : RoomManager
{
    [SerializeField] private Animator bossFSN;
    public override void StartRoom()
    {
        base.StartRoom();
        bossFSN.SetTrigger("EnterRoom");
    }
}
