using UnityEngine;
using UnityEngine.VFX;

public class SmokeDash : MonoBehaviour
{
    private VisualEffect dashParticle;

    void Start()
    {
        dashParticle = GetComponentInChildren<VisualEffect>();
        
        if (dashParticle == null)
        {
            Debug.LogError("No s'ha trobat el componente VisualEffect en els fills");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateDashParticle();
        }
    }

    void ActivateDashParticle()
    {
        if (dashParticle != null)
        {
            dashParticle.Play();
            Invoke("DeactivateDashParticle", 0.5f);
        }
    }

    void DeactivateDashParticle()
    {
        if (dashParticle != null)
        {
            dashParticle.Stop();
        }
    }
}