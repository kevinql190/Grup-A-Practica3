using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rollerBullet : MonoBehaviour
{
    public float speed = 10f;
    private bool collided = false;
    public GameObject target;
    private Rigidbody rb;
    public int damage = 1;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("rollerGround") && !collided)
        {
            collided = true;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = -transform.right * speed;
            }
        }
        Debug.Log(collision.gameObject);
        if (collision.gameObject.CompareTag("Player"))
        {
            target.GetComponent<IDamageable>().TakeDamage(-damage);
        }
    }
}
