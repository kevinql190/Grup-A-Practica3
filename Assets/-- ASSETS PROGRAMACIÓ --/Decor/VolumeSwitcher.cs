using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeSwitcher : MonoBehaviour
{
    public Volume volumeInicial;
    public Volume volumeTemporal;
    public float switchDuration = 5.0f; // Duración en segundos que el volumen temporal estará activo

    private void Start (){
        volumeInicial = GetComponent<Volume>();
        volumeTemporal = GetComponent<Volume>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el objeto tiene el tag "Player"
        {
            StartCoroutine(SwitchVolume());
        }
    }

    private IEnumerator SwitchVolume()
    {
        volumeInicial.enabled = false;
        volumeTemporal.enabled = true;

        yield return new WaitForSeconds(switchDuration);

        volumeTemporal.enabled = false;
        volumeInicial.enabled = true;
    }
}