using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionField : MonoBehaviour, IDamageable
{
    [SerializeField] private LayerMask warningLayers;
    [SerializeField] private float warningTime;
    [SerializeField] private float warningFresnelPower;
    [SerializeField] private float warningScrollSpeed;
    [SerializeField] private Color warningColor;
    [SerializeField] private AnimationCurve warningCurve;
    private Material fieldMaterial;
    private float startFresnelPower;
    private float startScrollSpeed;
    private Color startColor;
    private bool warningStarted;

    public int CurrentHealth { get ; set ; }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public bool TakeDamage(int damage)
    {
        if (!warningStarted) StartCoroutine(WarnSequence());
        return false;
    }

    private void Awake()
    {
        fieldMaterial = GetComponent<MeshRenderer>().material;
        startColor = fieldMaterial.GetColor("_Emission");
        startFresnelPower = fieldMaterial.GetFloat("_Fresnel_Power");
        startScrollSpeed = fieldMaterial.GetFloat("_Scroll_Speed");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if ((warningLayers & 1 << collision.gameObject.layer) != 0 && warningStarted)
        {
            StartCoroutine(WarnSequence());
        }
    }
    private IEnumerator WarnSequence()
    {
        warningStarted = true;
        float t = 0;
        while (t < warningTime)
        {
            t += Time.deltaTime;
            float value = warningCurve.Evaluate(t / warningTime);
            fieldMaterial.SetFloat("_Fresnel_Power", Mathf.Lerp(startFresnelPower, warningFresnelPower, value));
            fieldMaterial.SetFloat("_Scroll_Speed", Mathf.Lerp(startScrollSpeed, warningScrollSpeed, value));
            fieldMaterial.SetColor("_Emission", Color.Lerp(startColor, warningColor, value));
            yield return null;
        }
        warningStarted = false;
    }
}
