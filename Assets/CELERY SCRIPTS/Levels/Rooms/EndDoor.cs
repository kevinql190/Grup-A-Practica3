using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EndDoor : MonoBehaviour
{
    private BoxCollider _collider;
    private NavMeshObstacle _meshObstacle;
    [SerializeField] private Animator toastDoorObject;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _meshObstacle = GetComponent<NavMeshObstacle>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        transform.parent.GetComponentInParent<RoomManager>().ExitRoom();
        _meshObstacle.enabled = true;
        _collider.enabled = false;
        AudioManager.Instance.PlaySFXOnce("porta_tanca");
        toastDoorObject.SetTrigger("endclose");
    }
    private void OnDrawGizmos()
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        Gizmos.color = new Color(.2f, .5f, .5f, .2f);
        if (obstacle != null) Gizmos.DrawCube(transform.position + obstacle.center, Vector3.Scale(obstacle.size, transform.localScale));
        BoxCollider collider = GetComponent<BoxCollider>();
        Gizmos.color = new Color(1, 0, 0, .2f);
        if (collider != null) Gizmos.DrawCube(transform.position + collider.center, Vector3.Scale(collider.size, transform.localScale));
    }
}
