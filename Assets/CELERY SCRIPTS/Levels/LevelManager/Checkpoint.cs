using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GetComponent<BoxCollider>().enabled = false;
        CheckpointThis();
        transform.parent.GetComponentInParent<RoomManager>().EndRoom();
    }

    private void CheckpointThis()
    {
        CrossSceneInformation.CurrentCheckpoint = LevelManager.Instance.checkpoints.IndexOf(transform.parent.parent.gameObject);
    }
}
