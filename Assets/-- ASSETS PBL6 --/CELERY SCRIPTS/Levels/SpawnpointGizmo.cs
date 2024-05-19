using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointGizmo : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private bool isGizmoVisible = true;
    [SerializeField] private bool isForPlayer;
    private void OnDrawGizmos()
    {
        if (!isGizmoVisible) return;
        Gizmos.color = isForPlayer ? new Color(1, 0, 0, 0.6f) : new Color(1, 1, 1, 0.4f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}
