using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Cinemachine;

public class StartDoor : MonoBehaviour
{
    [SerializeField] private Animator toastDoorObject;
    [SerializeField] private bool opensAtEnd;
    [SerializeField] private float timeOfCameraChange = 1f;
    [SerializeField] private float cameraReturnDistance = 5f;
    private Transform playerPos;
    public void CloseDoor()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<NavMeshObstacle>().enabled = true;
        toastDoorObject.SetTrigger("startclose");
    }
    public void OpenDoor()
    {
        if (opensAtEnd)
        {
            GetComponent<NavMeshObstacle>().enabled = false;
            toastDoorObject.SetTrigger("open");
        }
        else
        {
            toastDoorObject.SetTrigger("exit");
        }
    }
    public void ChangeCamera()
    {
        if(opensAtEnd) StartCoroutine(ChangeCameraSequence());
    }

    private IEnumerator ChangeCameraSequence()
    {
        transform.parent.GetComponentInChildren<CinemachineVirtualCamera>().enabled = true;
        float startTime = Time.time;
        PlayerInputHandler.Instance.LockInputs();
        while (Time.time < startTime + timeOfCameraChange && Vector3.Distance(playerPos.position, transform.position) > cameraReturnDistance) yield return null;
        PlayerInputHandler.Instance.UnlockInputs();
        transform.parent.GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;
    }

    public void StopAnim()
    {
        toastDoorObject.SetTrigger("exit");
    }
    public void SetDoorCheckpoint()
    {
        if (opensAtEnd) toastDoorObject.SetTrigger("open");
        else toastDoorObject.SetTrigger("close");
    }
    private void OnDrawGizmos()
    {
        NavMeshObstacle modifier = GetComponent<NavMeshObstacle>();
        Gizmos.color = new Color(.2f, .5f, .5f, .2f);
        if (modifier != null) Gizmos.DrawCube(transform.position + modifier.center, Vector3.Scale(modifier.size, transform.localScale));
    }
}
