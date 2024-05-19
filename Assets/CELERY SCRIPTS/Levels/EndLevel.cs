using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour, IDamageable
{
    public int CurrentHealth { get; set; }

    public void Die()
    {
        LevelManager.Instance.EndLevel();
    }

    public bool TakeDamage(int damage)
    {
        Die();
        return true;
    }
}
