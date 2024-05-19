using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamageable>();
        if (target != null) target.TakeDamage(-1);
    }
}
