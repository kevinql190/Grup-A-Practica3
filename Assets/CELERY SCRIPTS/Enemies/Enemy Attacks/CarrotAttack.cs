using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrotAttack : Enemy
{
    [Header("Carrot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private SoundValues attackSound;

    [Header("Rotation Time")]
    public float rotationDuration = 0.2f;

    [Header("Bullet")]
    public float speedBullet = 10f;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    public override void Attack()
    {
        //Instancia Bullet
        GameObject bulletObj = Instantiate(bulletPrefab, spawnPoint.transform.position, Quaternion.Euler(0, 90, 0) * spawnPoint.transform.rotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletObj.GetComponent<Bullet>().damage = damage;
        bulletRig.AddForce(spawnPoint.forward * speedBullet, ForceMode.VelocityChange);
        AudioManager.Instance.PlaySFXOnce("pastanaga_enemic", 30);
    }
    // Potser posar al script de Enemy
    public override void FacePlayer()
    {
        StartCoroutine(RotateTowardsPlayer());
    }
    private IEnumerator RotateTowardsPlayer()
    {
        Vector3 direction = target.transform.position - transform.position;
        Vector3 projectedDirection = new Vector3(direction.x, 0, direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(projectedDirection);

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
