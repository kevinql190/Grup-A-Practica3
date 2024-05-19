using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private int bombDamage = 1;
    [SerializeField] private Color chargedColor;
    [SerializeField] private AnimationCurve colorChange;
    [SerializeField] private AnimationCurve bombMovement;
    [SerializeField] private GameObject particlesPrefab;
    [SerializeField] private float particlesTime;
    [SerializeField] private float timeToExplosion;
    private Color startColor;
    private Vector3 startPosition;
    private Material bombMaterial;
    private float radius;
    private void Awake()
    {
        bombMaterial = GetComponent<MeshRenderer>().material;
        startColor = bombMaterial.color;
        startPosition = transform.position;
    }
    public IEnumerator ChargeBomb(Vector3 targetPos, float bombRadius)
    {
        float t = 0;
        while (t < timeToExplosion)
        {
            t += Time.deltaTime;
            float valueColor = colorChange.Evaluate(t / timeToExplosion);
            bombMaterial.color = Color.Lerp(startColor, chargedColor, valueColor);
            float valueMovement = bombMovement.Evaluate(t / timeToExplosion);
            transform.position = Vector3.Lerp(startPosition, new Vector3(targetPos.x, transform.position.y, targetPos.z), valueMovement);
            yield return null;
        }
        Explode(bombRadius);
        GetComponent<MeshRenderer>().enabled = false;
        Instantiate(particlesPrefab, transform);
        yield return new WaitForSeconds(particlesTime);
        Destroy(gameObject);
    }
    private void Explode(float bombRadius)
    {
        Debug.Log("exploded");
        radius = bombRadius;
        var targets = Physics.OverlapSphere(transform.position, bombRadius, targetLayers);
        foreach(Collider target in targets)
        {
            target.GetComponent<IDamageable>().TakeDamage(-bombDamage);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
