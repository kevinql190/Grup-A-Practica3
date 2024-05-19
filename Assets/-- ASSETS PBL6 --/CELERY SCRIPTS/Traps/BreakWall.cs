using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviour
{
    [SerializeField] private GameObject breakableObjectPrefab;
    [SerializeField] public float destroyDelay = 3f;
    [Header("SFX")]
    [SerializeField] private SoundValues sound;
    private bool dishwasherController = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || (other.CompareTag("Dishwasher") && dishwasherController)) Destroy(gameObject);
        if (other.CompareTag("Wall") || (other.CompareTag("Dishwasher") && dishwasherController))
        {
            Quaternion rotation = transform.rotation * Quaternion.Euler(90, 0, 0);
            GameObject breakPrefab = Instantiate(breakableObjectPrefab, transform.position, rotation);
            if (sound != null) AudioManager.Instance.CreateAudioSource(transform, sound);
            Destroy(breakPrefab, destroyDelay);
            GetComponent<Collider>().enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dishwasher")) dishwasherController = true;
    }
}
