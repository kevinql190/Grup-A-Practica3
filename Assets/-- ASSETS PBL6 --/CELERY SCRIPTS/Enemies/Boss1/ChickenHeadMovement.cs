using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenHeadMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    private NavMeshAgent agent;
    [Header("Attack")]
    [SerializeField] private MeshRenderer chargeMaterial;
    [SerializeField] private float followTime;
    [SerializeField] private float attackChargeTime;
    [SerializeField] private float attackSpeed;
    [Header("Line")]
    [SerializeField] private float lineFadeDuration;
    private bool isAttacking = false;
    private Vector3 startingScale;
    private GameObject player;
    private LineRenderer lineRenderer;
    private IEnumerator vanishTrail;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        vanishTrail = VanishTrail();
        agent = GetComponent<NavMeshAgent>();
        startingScale = transform.localScale;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (isAttacking) return;
        if(Physics.Raycast(transform.position, transform.forward, out _, 100, LayerMask.GetMask("Player")))
        {
            StartCoroutine(ChargedAttackSequence());
        }
        else
        {
            FaceTarget();
        }
    }
    void FaceTarget()
    {
        Vector3 targetRotation = Quaternion.LookRotation(player.transform.position - transform.position).eulerAngles;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, targetRotation.y, transform.rotation.z), Time.deltaTime * rotationSpeed);
    }
    private IEnumerator ChargedAttackSequence()
    {
        isAttacking = true;
        StartCoroutine(DrawShootTrail());
        lineRenderer.widthMultiplier = 1;
        float t = 0;
        while (t < attackChargeTime)
        {
            t += Time.deltaTime;
            float value = t / attackChargeTime;
            transform.localScale = Vector3.Lerp(startingScale, new Vector3(5, 5, 5), value);
            chargeMaterial.material.SetFloat("_Controlador", value);
            FaceTarget();
            LineFollow();
            yield return null;
        }
        transform.localScale = startingScale;
        chargeMaterial.material.SetFloat("_Controlador", 0);
        agent.speed = attackSpeed;
        t = 0;
        while(NavMesh.SamplePosition(transform.position + transform.forward * 3 - Vector3.up * agent.height / 2 * transform.localScale.y, out _, 1.5f, NavMesh.AllAreas))
        {
            t += Time.deltaTime;
            if (t < followTime)
            {
                LineFollow();
                FaceTarget();
            }
            agent.Move(transform.forward);
            yield return null;
        }
        isAttacking = false;
    }
    private void LineFollow()
    {
        lineRenderer.SetPosition(0, transform.position - Vector3.up * agent.height / 2 * transform.localScale.y);
        Vector3 thisLocation = transform.position - Vector3.up * agent.height / 2 * transform.localScale.y;
        Vector3 targetLocation = transform.position + transform.forward * 100 - Vector3.up * agent.height / 2 * transform.localScale.y;
        if (NavMesh.Raycast(thisLocation, targetLocation, out NavMeshHit hit, NavMesh.GetAreaFromName("boss")))
            lineRenderer.SetPosition(1, hit.position);
        else
            lineRenderer.SetPosition(1, targetLocation);
    }
    private IEnumerator DrawShootTrail()
    {
        LineFollow();
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(attackChargeTime);
        StopCoroutine(vanishTrail);
        vanishTrail = VanishTrail();
        StartCoroutine(vanishTrail);
    }
    private IEnumerator VanishTrail()
    {
        float t = 0;
        while (t < lineFadeDuration)
        {
            t += Time.deltaTime;
            float value = t / lineFadeDuration;
            lineRenderer.widthMultiplier = 1 - value;
            yield return null;
        }
    }
}
