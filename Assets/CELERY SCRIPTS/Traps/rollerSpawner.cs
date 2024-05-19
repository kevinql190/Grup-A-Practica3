using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rollerSpawner : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public GameObject target;
    [SerializeField] public float timeBetweenShoot = 2f;
    private float timeSinceLastShot;

    [Header("Roller")]
    public int damage = 1;
    public float speedBullet = 10f;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
    }
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= timeBetweenShoot)
        {
            StartCoroutine(AttackWithDelay());
            timeSinceLastShot = 0f;
        }
    }

    IEnumerator AttackWithDelay()
    {
        yield return new WaitForSeconds(0.05f);
        //Instancia Bullet

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
    }

    public void Deactivate()
    {
        this.enabled = false;
    }
}
