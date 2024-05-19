using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjects : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject breakableObjectPrefab;
    [SerializeField] public Material breakMaterial;
    [SerializeField] public float destroyDelay = 3f;
    [Header("SFX")]
    [SerializeField] private SoundValues soundAtDestroy;
    public int CurrentHealth { get; set; } = 1;
    public int maxHealth = 2;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }
    private void BreakTheThing()
    {
        Quaternion rotation = transform.rotation * Quaternion.Euler(90, 0, 0); //S'ha de treure quan es tinguin les cadires/ taules amb les mides corresponents
        GameObject breakPrefab = Instantiate(breakableObjectPrefab, transform.position, rotation);
        soundAtDestroy.sound = "trenca_fusta_" + UnityEngine.Random.Range(1, 4).ToString();
        AudioManager.Instance.CreateAudioSource(transform, soundAtDestroy);
        Destroy(breakPrefab, destroyDelay); 
        Destroy(gameObject);
    }

    public bool TakeDamage(int damage)
    {
        CurrentHealth += damage;
        if (CurrentHealth < 1)
        {
            Die();
        }
        else 
        {
            Chrack();
        }
        return true;
    }
    private void Chrack()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = breakMaterial;
        }
    }
    public void Die()
    {
        BreakTheThing();
    }
}
