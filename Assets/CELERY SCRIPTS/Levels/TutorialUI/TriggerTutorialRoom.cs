using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorialRoom : MonoBehaviour
{
    [SerializeField] Collider checkpointCollider;
    [SerializeField] GameObject pauseTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointCollider.enabled = false;
            pauseTrigger.SetActive(true);
        }
        StartCoroutine(WaitForPause());
    }
    private IEnumerator WaitForPause()
    {
        while (Time.timeScale != 0)
        {
            yield return null;
        }
        checkpointCollider.enabled = true;
        Destroy(pauseTrigger);
    }
}
