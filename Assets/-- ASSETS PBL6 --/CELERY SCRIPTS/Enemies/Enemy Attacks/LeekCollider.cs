using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeekCollider : MonoBehaviour
{
    public Animator animator;
    public GameObject target;
    [Header("Rotation Time Attack Leek")]
    public float rotationAttackTime = 1.8f;

    public IEnumerator RotateAndAttack()
    {
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(90, initialRotation.eulerAngles.y, initialRotation.eulerAngles.z);
        float startTime = Time.time;
        float rotationDuration = rotationAttackTime;

        while (Time.time - startTime < rotationDuration)
        {
            float t = (Time.time - startTime) / rotationDuration;
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;

        // Después de la rotación, establece la rotación de vuelta a la inicial
        transform.rotation = initialRotation;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    void OnTriggerEnter(Collider Collider)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Debug.Log("Damahe");
            if (Collider.gameObject.CompareTag("Player"))
            {
                target.GetComponent<IDamageable>().TakeDamage(-animator.GetComponent<Enemy>().damage);
            }
            if (Collider.gameObject.layer == LayerMask.NameToLayer("Breakables"))
            {
                Debug.Log("aaaaaaaaaa");
                Collider.GetComponent<IDamageable>().TakeDamage(-animator.GetComponent<Enemy>().damage);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Breakables"))
            {
                Debug.Log("aaaaaaaaaa");
                other.GetComponent<IDamageable>().TakeDamage(-animator.GetComponent<Enemy>().damage);
            }
        }
    }
}
