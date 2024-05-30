using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX; // Asegúrate de incluir esta línea para usar Visual Effects

public class ParticleOnion : MonoBehaviour
{
    [SerializeField] private VisualEffect explosionEffect; // Referencia al Visual Effect de explosión

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle Collision Detected with: " + other.name);

        // Instanciar el efecto de explosión en el punto de colisión
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
