using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TomatoAttack : Enemy
{
    private Collider attackCollider;

    [Header("Tomato Sphere Attack")]
    public float sphereRadius = 0.6f;
    public float distanceAhead = 0.9f;
    public float distanceAbove = 0.6f;

    [Header("Rotation Time")]
    public float rotationDuration = 0.2f;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    public override void Attack()
    {
        CreateAttackCollider();
    }

    private void CreateAttackCollider()
    {
        if (attackCollider != null)
        {
            Destroy(attackCollider.gameObject);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * distanceAhead + Vector3.up * distanceAbove, sphereRadius);

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                Damager();
            }
            if (collider.gameObject.layer == LayerMask.NameToLayer("Breakables"))
            {

                collider.GetComponent<IDamageable>().TakeDamage(-damage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * distanceAhead + Vector3.up * distanceAbove, sphereRadius);
    }

    // Potser posar al script de Enemy
    public override void FacePlayer()
    {
        StartCoroutine(RotateTowardsPlayer());
    }
    private IEnumerator RotateTowardsPlayer()
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / rotationDuration);
            transform.rotation = newRotation;
            yield return null;
        }
        transform.rotation = targetRotation;
    }
}