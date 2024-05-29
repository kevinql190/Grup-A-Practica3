using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class VolumeSwitcher : MonoBehaviour
{
    public Volume volume;
    public VolumeProfile profileInicial;
    public VolumeProfile profileTemporal;
    public float switchDuration = 5.0f; // Duración en segundos que el perfil temporal estará activo

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SwitchVolumeProfile());
        }
    }

    private IEnumerator SwitchVolumeProfile()
    {
        Debug.Log("Activando perfil temporal...");
        volume.profile = profileTemporal;

        yield return new WaitForSeconds(switchDuration);

        Debug.Log("Restaurando perfil inicial...");
        volume.profile = profileInicial;
    }
}
