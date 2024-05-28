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

    // EXTRA - Realizar acciones según la opción en medio del diálogo
    [Header("Action Nodes")]
    public DialogueNode carrotNode1_1;
    public DialogueNode carrotNode2_2;
    public DialogueNode carrotNode3;

        // NPC se vuelve rojo
    [Header("Texture Change")]
    public Material redMaterial;
    private Renderer objectRenderer;
    private Material originalMaterial;
        // Símbolo exclamación/ emoji
    public GameObject exclamation;
    public GameObject happyEmoji;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        animator = GetComponent<Animator>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
        objectRenderer = GetComponentInChildren<Renderer>();
        originalMaterial = objectRenderer.material;

        // EXTRA - Realizar acciones según la opción en medio del diálogo
            //Emoji
        carrotNode1_1.NodeAction = (GameObject talker) => {
            if (happyEmoji != null) {
                happyEmoji.SetActive(true);
                StartCoroutine(DisableCanvasAfterDelay(happyEmoji));
            }
        };
            // NPC se pone rojo
        carrotNode2_2.NodeAction = (GameObject talker) => {
            StartCoroutine(TurnRedForTime());
        };
            // Símbolo de exclamación
        carrotNode3.NodeAction = (GameObject talker) => {
            if (exclamation != null)
            {
                exclamation.SetActive(true);
                StartCoroutine(DisableCanvasAfterDelay(exclamation));
            }
        };
    }

    // ------------ EXTRA : NPC se vuelve de color rojo ---------------
    private IEnumerator TurnRedForTime()
    {
        objectRenderer.material = redMaterial;
        yield return new WaitForSeconds(1.5f);
        objectRenderer.material = originalMaterial;
    }
    //-----------------------------------------------------------------

    // ------------ EXTRA : Desactivar canvas ---------------
    private IEnumerator DisableCanvasAfterDelay(GameObject canvas)
    {
        yield return new WaitForSeconds(1.5f); 

        if (canvas != null)
        {
            // Desactiva el canvas
            canvas.SetActive(false);
        }
    }
    //-----------------------------------------------------------------

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
        AudioManager.Instance.PlayMusicLoop("cancionZanahoria");
        AudioManager.Instance.StopMusicLoop(16.2f);
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
                GameObject newObject = Instantiate(spawnPoint.prefabToSpawn, spawnPoint.spawnLocation.position, spawnPoint.spawnLocation.rotation);
            }
        }
    }
    //--------------------------------------------------------------
}
