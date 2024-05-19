using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private int targetCount = 1;
    [SerializeField] private float lifeTimeBullet = 3f;
    [SerializeField] private bool destroyWhenHitWall = true;
    [Header("SFX")]
    [SerializeField] private SoundValues soundAtDestroy;
    [SerializeField] private float soundDamageVolume;
    public int damage = 1;
    private void Start()
    {
        if (lifeTimeBullet != -1) Destroy(gameObject, lifeTimeBullet);
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageReceiver = other.GetComponent<IDamageable>();
        bool shouldDamage = (targetLayers.value & 1 << other.gameObject.layer) != 0;
        if ((targetLayers.value & 1 << LayerMask.NameToLayer("Player")) != 0 && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.GetComponent<PlayerMovement>().IsDashing) return; // El dash no afecta els projectils
        }
        if (damageReceiver != null && shouldDamage)
        {
            if ((targetLayers.value & 1 << LayerMask.NameToLayer("Enemies")) != 0 && other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
                AudioManager.Instance.PlaySFXRisingPitch("habilitat_impacta_enemic", soundDamageVolume, maxPitchRiseCount:5);
            damageReceiver.TakeDamage(-damage);
            targetCount--;
        }
        if ((other.CompareTag("Wall") && destroyWhenHitWall) || targetCount < 1)
        {
            if (!string.IsNullOrEmpty(soundAtDestroy.sound)) AudioManager.Instance.CreateAudioSource(transform, soundAtDestroy);
            Destroy(gameObject);
        }
    }
}
