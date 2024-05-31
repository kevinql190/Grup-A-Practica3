using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem healParticles;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            int healthdifference = playerHealth.maxHealth - playerHealth.CurrentHealth;
            if (healthdifference == 0)
            {
                StartBurst(10, new Color32(255, 255, 255, 165));
            }
            else
            {
                StartBurst(10 * healthdifference, new Color32(176, 248, 172, 165));
                playerHealth.CurrentHealth = playerHealth.maxHealth;
            }
        }
    }
    private void StartBurst(float burstCount, Color color)
    {
        ParticleSystem.MainModule main = healParticles.main;
        main.startColor = color;
        var em = healParticles.emission;
        em.SetBursts(
            new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, _count:burstCount, 1, 0.01f)
            });
        healParticles.Play();
    }
}
