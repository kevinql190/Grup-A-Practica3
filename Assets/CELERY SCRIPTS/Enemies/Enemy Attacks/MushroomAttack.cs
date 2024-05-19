using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAttack : Enemy
{
    [Header("Wave Mushroom")]
    public float minimRadius = 1.5f;
    public float maximRadius = 4f;
    public float thicknessAreaDamage = 0.5f;
    public float waveDuration = 1f;

    private float currentRadius;
    private bool isAttacking;

    [Header("Rotation Time")]
    public float rotationDuration = 0.2f;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    public override void Attack()
    {
        StartCoroutine(WaveAttack());
    }

    private IEnumerator WaveAttack()
    {
        float elapsedTime = 0f;
        isAttacking = true;

        while (elapsedTime < waveDuration)
        {
            currentRadius = Mathf.Lerp(minimRadius, maximRadius, elapsedTime / waveDuration);
            DetectAndDamage(currentRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentRadius = maximRadius;
        DetectAndDamage(currentRadius);

        isAttacking = false;
    }

    private void DetectAndDamage(float currentRadius)
    {
        float innerRadius = currentRadius - thicknessAreaDamage / 2;
        float outerRadius = currentRadius + thicknessAreaDamage / 2;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, outerRadius);
        foreach (var hitCollider in hitColliders)
        {
            float distanceToTarget = Vector3.Distance(transform.position, hitCollider.transform.position);
            if (distanceToTarget >= innerRadius && distanceToTarget <= outerRadius)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    Damager();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minimRadius);
        Gizmos.DrawWireSphere(transform.position, maximRadius);

        if (isAttacking)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, currentRadius - thicknessAreaDamage / 2);
            Gizmos.DrawWireSphere(transform.position, currentRadius + thicknessAreaDamage / 2);
        }
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

