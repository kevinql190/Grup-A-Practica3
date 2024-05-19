using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomTrigger : MonoBehaviour
{
    private BoxCollider _collider;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        CrossSceneInformation.CurrentRoom = LevelManager.Instance.rooms.IndexOf(transform.parent.parent.gameObject);
        transform.parent.GetComponentInParent<RoomManager>().StartRoom();
        _collider.enabled = false;
    }
    private void OnDrawGizmos()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        Gizmos.color = new Color(1,0,0,.2f);
        if (collider != null) Gizmos.DrawCube(transform.position + collider.center, Vector3.Scale(collider.size, transform.localScale));
    }
}
