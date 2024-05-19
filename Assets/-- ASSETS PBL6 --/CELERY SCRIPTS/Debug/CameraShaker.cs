using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private GameObject cameraGameObject;
    [SerializeField] private float intensity;
    [SerializeField] private float duration;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        cameraGameObject.GetComponent<CameraController>().CameraShake(intensity, duration);
    }
}
