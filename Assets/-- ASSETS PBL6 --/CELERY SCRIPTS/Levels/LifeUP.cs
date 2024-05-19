using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUP : MonoBehaviour
{
    [SerializeField] private int lifeQuantity;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(lifeQuantity);
            other.GetComponent<IDamageable>().TakeDamage(lifeQuantity);
            Destroy(gameObject);
        }
    }
}
