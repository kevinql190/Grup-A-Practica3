using UnityEngine;
using System.Collections;

public class ChangeMaterialOnProximity : MonoBehaviour
{
    public Material material;  // Material a modificar
    public float metallicClose = 1.0f;  // Valor de metallic cuando el personaje está cerca
    public float metallicFar = 0.0f;  // Valor de metallic cuando el personaje está lejos
    public float proximityThreshold = 5.0f;  // Distancia para considerar que el personaje está cerca
    public float transitionDuration = 1.0f;  // Duración de la transición

    private GameObject player;
    private Renderer objectRenderer;
    private Coroutine changingMetallic;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");  // Asegúrate de que tu personaje tenga el tag "Player"
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null && objectRenderer.material != null)
        {
            material = objectRenderer.material;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        float targetMetallic = (distance < proximityThreshold) ? metallicClose : metallicFar;

        if (changingMetallic != null) StopCoroutine(changingMetallic);
        changingMetallic = StartCoroutine(ChangeMetallic(targetMetallic));
    }

    private IEnumerator ChangeMetallic(float targetMetallic)
    {
        float t = 0;
        float initialMetallic = material.GetFloat("_Metallic");

        while (t < transitionDuration)
        {
            t += Time.deltaTime;
            float value = Mathf.Lerp(initialMetallic, targetMetallic, t / transitionDuration);
            material.SetFloat("_Metallic", value);
            yield return null;
        }
        material.SetFloat("_Metallic", targetMetallic);  // Asegura el valor final
    }
}
