using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointRoom : RoomManager
{
    public override void StartRoom()
    {
        base.StartRoom();
        StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().ResetHearts());
    }
    public void RespawnSetDoors()
    {
        foreach (StartDoor door in transform.GetComponentsInChildren<StartDoor>())
        {
            door.CloseDoor();
        }
    }
}
