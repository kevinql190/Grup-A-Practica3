using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dishwasher : MonoBehaviour, ITrap
{
    [Header("Dishwasher")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public GameObject target;
    [SerializeField] public float timeBetweenShoot = 2f;
    public float attackRange = 5f;
    private float timeSinceLastShot;

    [Header("Dish")]
    public int damage = 1;
    public float speedBullet = 10f;

    [Header("Animation")]
    private Animator dishwasherAnimator;
    [SerializeField] private string animationOpen;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
        dishwasherAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (InRange())
        {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot >= timeBetweenShoot)
            {
                StartCoroutine(AttackWithDelay());
                timeSinceLastShot = 0f;
            }
        }
    }
    public bool InRange()
    {
        return Vector3.Distance(target.transform.position, transform.position) <= attackRange;
    }
    IEnumerator AttackWithDelay()
    {
        //Animació open
        dishwasherAnimator.Play(animationOpen);
        yield return new WaitForSeconds(0.05f);
        //Instancia Bullet
        Quaternion bulletRotation = Quaternion.Euler(-90, 0, 0);

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation * bulletRotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletObj.GetComponent<Bullet>().damage = damage;
        bulletRig.AddForce(spawnPoint.forward * speedBullet, ForceMode.VelocityChange);
    }

    public void Deactivate()
    {
        enabled = false;
    }
}
