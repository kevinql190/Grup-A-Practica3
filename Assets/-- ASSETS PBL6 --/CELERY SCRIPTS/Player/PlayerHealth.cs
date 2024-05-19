using System.Collections;
using UnityEngine;
using System;
using Cinemachine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Life")]
    public int maxHealth;
    public Cooldown damageCooldown;
    public event Action<int> OnHealthChanged;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; OnHealthChanged?.Invoke(value); }
    }
    private int _currentHealth;
    private PlayerMovement _playerMovement;
    [Header("Death")]
    [SerializeField] private bool canDie;
    [SerializeField] private float deathDuration = 2f;
    private bool isDead;
    public event Action OnPlayerDeath;
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private AnimationCurve damageCurve;
    private void Start()
    {
        CurrentHealth = maxHealth;
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public IEnumerator ResetHearts()
    {
        while (CurrentHealth < maxHealth)
        {
            CurrentHealth++;
            yield return new WaitForSeconds(1f);
        }
    }
    public bool TakeDamage(int damage)
    {
        if (_playerMovement.IsDashing || damageCooldown.IsCoolingDown) { Debug.Log("Invulnerable"); return false; }
        AudioManager.Instance.PlaySFXOnce("player_hit", 3.5f);
        CurrentHealth += damage;
        if (CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
        }
        else if (CurrentHealth < 1)
        {
            if(!isDead) Die();
        }
        damageCooldown.StartCooldown();
        if (damage > 0) return false;
        StartCoroutine(DamageMaterialChange());
        GetComponent<PlayerAnimatorManager>().TriggerAnimation("hit");
        return true;
    }

    private IEnumerator DamageMaterialChange()
    {
        float t = 0;
        damageMaterial.SetFloat("_Type", 1);
        while (t < damageCooldown.CooldownTime)
        {
            t += Time.deltaTime;
            float value = damageCurve.Evaluate(t / damageCooldown.CooldownTime);
            if (damageMaterial != null) damageMaterial.SetFloat("_Controlador", value);
            yield return null;
        }
    }

    public void Die()
    {
        if (!canDie) return;
        animator.SetBool("death", true);
        isDead = true;
        StartCoroutine(DieSequence());
    }
    private IEnumerator DieSequence()
    {
        AudioManager.Instance.StopAllLoops(0);
        AudioManager.Instance.StopMusicLoop(0);
        AudioManager.Instance.PlaySFXOnce("player_mort");
        PlayerInputHandler.Instance.DisableInputs();
        GameManager.Instance.ChangeTimeScale(0, deathDuration);
        CinemachineVirtualCamera playerCam = (CinemachineVirtualCamera)GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        yield return StartCoroutine(playerCam.GetComponent<CameraController>().ChangeOrthoSize(2.5f, deathDuration));
        AudioManager.Instance.StopAllLoops(1f);
        OnPlayerDeath?.Invoke();
    }
    private void OnDisable()
    {
        if (damageMaterial != null) damageMaterial.SetFloat("_DamageValue", 0f);
    }
}

