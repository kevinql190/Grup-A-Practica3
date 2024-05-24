using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX; 

public class ParticleRock : MonoBehaviour
{
    [SerializeField] private VisualEffect explosionEffect; 

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle Collision Detected with: " + other.name);

        InstantiateExplosion(other.transform.position);
    }

    private void InstantiateExplosion(Vector3 position)
    {
        if (explosionEffect != null)
        {
            VisualEffect explosion = Instantiate(explosionEffect, position, Quaternion.identity);
            explosion.Play();
            Debug.Log("Explosion instantiated and played.");
        }
        else
        {
            Debug.LogError("Explosion effect prefab is not assigned.");
        }
    }
}

