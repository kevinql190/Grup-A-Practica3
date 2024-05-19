using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] GameObject bullet;

    private void Update()
    {
        // Rotar la bala en el eje especificado
        bullet.transform.localRotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
    }
}
