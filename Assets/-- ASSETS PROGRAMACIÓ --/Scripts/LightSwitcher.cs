using UnityEngine;
using System.Collections;

public class LightSwitcher : MonoBehaviour
{
    public Light currentLight;
    public Light interiorRef;
    public Light exteriorRef;
    public float switchDuration = 3.0f;
    IEnumerator changingLight;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(changingLight!=null) StopCoroutine(changingLight);
            changingLight = SwitchLight(interiorRef);
            StartCoroutine(changingLight);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (changingLight != null) StopCoroutine(changingLight);
            changingLight = SwitchLight(exteriorRef);
            StartCoroutine(changingLight);
        }
    }

    private IEnumerator SwitchLight(Light lightToSwitch)
    {
        float t = 0;
        Quaternion startRotation = currentLight.transform.rotation;
        Color startColor = currentLight.color;
        float startTemperature = currentLight.colorTemperature;
        float startIntensity = currentLight.intensity;
        float startShadowStrength = currentLight.shadowStrength;
        while (t<switchDuration)
        {
            t += Time.deltaTime;
            float value = t / switchDuration;
            currentLight.transform.rotation = Quaternion.Lerp(startRotation, lightToSwitch.transform.rotation, value);
            currentLight.color = Color.Lerp(startColor, lightToSwitch.color, value);
            currentLight.colorTemperature = Mathf.Lerp(startTemperature, lightToSwitch.colorTemperature, value);
            currentLight.intensity = Mathf.Lerp(startIntensity, lightToSwitch.intensity, value);
            currentLight.shadowStrength = Mathf.Lerp(startShadowStrength, lightToSwitch.shadowStrength, value);
            yield return null;
        }
    }
}
