using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomManager : MonoBehaviour
{
    [Header("Dissolve Material")]
    [SerializeField] private float dissolveTime = 1;
    [SerializeField] private Material dissolveShader;
    [Header("Room Objects")]
    [SerializeField] private List<GameObject> enterRooms;
    [SerializeField] private List<GameObject> exitRooms;
    public Transform respawnPoint;
    [SerializeField] private FoodType anyFoodToUnlockThisRoom;
    virtual protected void Awake()
    {
        if (respawnPoint == null) Debug.LogWarning("No respawnpoint in room " + gameObject);
    }
    virtual public void StartRoom()
    {
        StartCoroutine(RoomEnterAndLeave(true));
        StartCoroutine(RoomSequence());
        if (anyFoodToUnlockThisRoom != FoodType.Default) GameManager.Instance.UnlockFood(anyFoodToUnlockThisRoom);
        LevelManager.Instance.isTimerON = true;
    }
    virtual protected IEnumerator RoomSequence()
    {
        yield break;
    }
    virtual public void EndRoom()
    {
        LevelManager.Instance.isTimerON = false;
        StopCoroutine(RoomSequence());
        StartCoroutine(RoomEnterAndLeave(false));
    }
    virtual public void ExitRoom()
    {

    }
    IEnumerator RoomEnterAndLeave(bool isEnter)
    {
        if (isEnter)
        {
            foreach (StartDoor door in transform.GetComponentsInChildren<StartDoor>())
            {
                AudioManager.Instance.PlaySFXOnce("porta_tanca_eco");
                door.CloseDoor();
            }
            StartCoroutine(Dissolve(true));
            yield return new WaitForSeconds(dissolveTime);
            foreach(GameObject room in enterRooms)
            {
                room.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject room in exitRooms)
            {
                room.SetActive(true);
            }
            StartCoroutine(Dissolve(false));
            yield return new WaitForSeconds(dissolveTime);
            foreach (StartDoor door in transform.GetComponentsInChildren<StartDoor>())
            {
                AudioManager.Instance.PlaySFXOnce("porta_obre");
                door.OpenDoor();
            }
        }
    }
    #region Room Exterior Dissolve
    public IEnumerator Dissolve(bool willDisapear)
    {
        float t = 0f;
        while (t < dissolveTime)
        {
            t += Time.unscaledDeltaTime;
            float value = Mathf.InverseLerp(0f, dissolveTime, t);
            dissolveShader.SetFloat("_Dissolve", willDisapear ? value : (1 - value));
            yield return null;
        }
    }
    #endregion
}
