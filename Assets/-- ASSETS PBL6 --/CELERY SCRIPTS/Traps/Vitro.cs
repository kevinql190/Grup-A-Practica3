using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vitro : MonoBehaviour
{
    [Header("Vitro")]
    public int damage = 1;
    [SerializeField] private GameObject target;
    public float vitroCooldownDamage = 2f;
    private bool damaging = false;

    [Header("Turned On/ Off")]
    [SerializeField] public bool StartActivated;
    // Tiempo para activar la vitro
    public float activationTime = 5f;
    // Tiempo para desactivar la vitro
    public float desactivationTime = 5f; 

    public Material defaultMaterial;
    public Material newMaterial;
    public Color targetColor;
    private Color originalColor;

    public List<GameObject> objectsToChangeMaterial = new List<GameObject>();

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        originalColor = defaultMaterial.GetColor("_BaseColor");
        if (StartActivated)
        {
            Invoke(nameof(TurnedOn), 0f);
        }
        else 
        {
            Invoke(nameof(TurnedOff), 0f);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !damaging)
        {
            damaging = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damaging = false;
        }
    }

    private IEnumerator DamageCoroutine()
    {
        while (StartActivated)
        {
            if (damaging)
            {
                if (target.GetComponent<IDamageable>().TakeDamage(-damage))
                {
                    Debug.Log("started");
                    yield return new WaitForSeconds(vitroCooldownDamage);
                    Debug.Log("ended");
                }
            }
            yield return null;
        }
    }

    void TurnedOn()
    {
        StartActivated = true;
        ChangeMaterials(newMaterial, objectsToChangeMaterial);
        AdjustLightIntensity(1f);
        StopCoroutine(DamageCoroutine());
        StartCoroutine(DamageCoroutine());
        Invoke(nameof(TurnedOff), activationTime);
    }

    void TurnedOff()
    {
        StartActivated = false;
        defaultMaterial.SetColor("_BaseColor", originalColor);
        StartCoroutine(StartTransitionAfterDelay(defaultMaterial, targetColor, desactivationTime * 0.5f, desactivationTime)); 
        ChangeMaterials(defaultMaterial, objectsToChangeMaterial);
        AdjustLightIntensity(14f);

        Invoke(nameof(TurnedOn), desactivationTime);
    }

    // Transición
    IEnumerator StartTransitionAfterDelay(Material material, Color targetColor, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(ChangeMaterialColorGradually(material, targetColor, duration));
    }
    IEnumerator ChangeMaterialColorGradually(Material material, Color targetColor, float duration)
    {
        Color startColor = material.GetColor("_BaseColor");
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            material.SetColor("_BaseColor", Color.Lerp(startColor, targetColor, t));
            yield return null;
        }

        material.SetColor("_BaseColor", targetColor); 
    }

    // Cambio material/ intensidad
    void ChangeMaterials(Material material, List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = material;
            }
        }
    }
    void AdjustLightIntensity(float intensity)
    {
        foreach (GameObject obj in objectsToChangeMaterial)
        {
            Light lightComponent = obj.GetComponent<Light>();
            if (lightComponent != null)
            {
                lightComponent.intensity = intensity;
            }
        }
    }

    //Restaurar el color del material de la vitro apagada
    void OnApplicationQuit()
    {
        defaultMaterial.SetColor("_BaseColor", originalColor);
    }
    public void Deactivate()
    {
        this.enabled = false;
    }
}
