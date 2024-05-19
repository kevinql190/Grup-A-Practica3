using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IStealFoodType
{
    [Header("Enemy")]
    public FoodScriptableObject EnemyType;
    public int CurrentHealth { get; set; }
    [Header("Attack")]
    public int damage = 1;
    public GameObject target; 
    public float attackRange = 5f;
    public float detectionDistance = 10f;
    public float prepareAttackTime = 0.5f;
    public float cooldownAttack = 1f;
    public float stoppingDistanceAttack = 1f; //rang en el que es para el navagent abans de preparar-se per atacar
    private bool isDead = false;

    [Header("Dead Particle")]
    public GameObject deathParticlesPrefab;

    private void Awake()
    {
        CurrentHealth = EnemyType.enemyHealth;
    }
    public virtual void Attack() { }
    public virtual void FacePlayer() { }
    public virtual void Damager() 
    {
        target.GetComponent<IDamageable>().TakeDamage(-damage);
    }
    public bool InRange()
    {
        return Vector3.Distance(target.transform.position, transform.position) <= attackRange;
    }
    public virtual bool TakeDamage(int damage)
    {
        if (CurrentHealth > 1) //Damage
        {
            CurrentHealth += damage;
            //Efecte damage i animació
        }
        else //Mort
        {
            Die();
        }
        return true;
    }
    public void Die()
    {
        isDead = true;
        AudioManager.Instance.PlaySFXOnce("mort_enemic");
        Array.Find(GameManager.Instance.receptariInfo, x => x.FoodType == EnemyType).cookCount += 1;
        if (deathParticlesPrefab != null)
        {
            GameObject particlesInstance = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(particlesInstance, 3f);
        }
        Destroy(transform.parent.gameObject);
    }
    public void StealFoodType(PanController panController)
    {
        if (isDead)
        {
            panController.ChangeFoodType(EnemyType.FoodType);
            AudioManager.Instance.PlaySFXOnce("atac_colpeja");
        }
    }

}
