using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SpawnPoint
{
    public Transform spawnLocation;
    public GameObject prefabToSpawn;
}
public class Actions : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private NavMeshObstacle navMeshObstacle;
    public Transform destinationObject;
    private Animator animator;
    private DialogueTrigger dialogueTrigger;
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        animator = GetComponent<Animator>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    //----- El NPC se mueve hacia un objeto concreto (el tomate) ------
    public void Move()
    {
        StartCoroutine(MoveAfterDelay(1.0f));
    }
    private IEnumerator MoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (navMeshObstacle != null)
        {
            navMeshObstacle.enabled = false;
        }

        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;

            if (destinationObject != null)
            {
                navMeshAgent.SetDestination(destinationObject.position);
            }
        }
    }
    //--------------------------------------------------------------

    //----------------------- Sound Music --------------------------
    public void SoundMusic() 
    {
        AudioManager.Instance.PlayMusicLoop("MenuThemeCelery");
    }
    //--------------------------------------------------------------

    //----------------------- Carrot Attack --------------------------
    public void Attack()
    {
        StartCoroutine(AttackAfterDelay(1.5f));
    }

    private IEnumerator AttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (dialogueTrigger != null)
        {
            dialogueTrigger.enabled = false;
            dialogueTrigger.DisableTrigger();
        }
        if (navMeshObstacle != null)
        {
            navMeshObstacle.enabled = false;
        }
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
        }
        if (animator != null)
        {
            animator.enabled = true; 
        }
        
    }
    //--------------------------------------------------------------

    //----------------------- Spawn Carrots --------------------------
    public void Spawn() 
    {
        StartCoroutine(SpawnAfterDelay(1.0f));
    }
    private IEnumerator SpawnAfterDelay(float delay) 
    {
        yield return new WaitForSeconds(delay);
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.prefabToSpawn != null && spawnPoint.spawnLocation != null)
            {
                // Instancia una copia del prefab en la posición y rotación del spawn point
                GameObject newObject = Instantiate(spawnPoint.prefabToSpawn, spawnPoint.spawnLocation.position, spawnPoint.spawnLocation.rotation);

                // Opcional: Puedes hacer más configuraciones en la nueva instancia aquí
                // newObject.name = "NombrePersonalizado";
                // newObject.GetComponent<SomeComponent>().SomeProperty = someValue;

                Debug.Log("Instanciado " + newObject.name + " en " + spawnPoint.spawnLocation.position);
            }
        }
    }
    //--------------------------------------------------------------
}
