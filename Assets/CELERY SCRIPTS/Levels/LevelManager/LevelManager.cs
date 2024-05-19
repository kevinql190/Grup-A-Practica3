using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Debug")]
    public bool isDebugging;
    [SerializeField] GameObject startRoomGameObject;
    [SerializeField] private bool isCinematicON;

    public Action<bool> OnStartLevel;
    [Header("Dissolve Material")]
    [SerializeField] private Material dissolveShader;
    [Header("Level")]
    [SerializeField] private int levelId;
    [Header("StartLevel")]
    public float startTime = 4f;
    public float startOrthoSize = 15f;
    [Header("Rooms")]
    public List<GameObject> rooms;
    public List<GameObject> checkpoints;
    private GameObject currentRoom;
    private GameObject roomToActivate;
    //Level End
    public Action OnLevelEnd;
    [Header("Timer")]
    [HideInInspector] public float elapsedTime;
    [HideInInspector] public bool isTimerON;
    private void Awake()
    {
        if (dissolveShader != null) dissolveShader.SetFloat("_Dissolve", 0);
        if (CrossSceneInformation.CurrentLevel == -1) CrossSceneInformation.CurrentLevel = levelId;
        elapsedTime = CrossSceneInformation.CurrentTimerValue;
        if (checkpoints.Count == 0)
            Debug.LogWarning("No checkpoints in LevelManager");
        roomToActivate = rooms[CrossSceneInformation.CurrentRoom];
        ActivateRooms();
    }

    private void Start()
    {
        PlayerInputHandler.Instance.playerInput.SwitchCurrentActionMap("Gameplay");
        Cursor.lockState = CursorLockMode.Locked;

        TeleportToLastRoom();

        OnStartLevel?.Invoke(elapsedTime == 0 && isCinematicON);
    }
    private void Update()
    {
        if (isTimerON) elapsedTime += Time.deltaTime;
    }
    #region Rooms
    private void ActivateRooms()
    {
        if (startRoomGameObject != null && CrossSceneInformation.CurrentRoom == 0) currentRoom = startRoomGameObject;
        else currentRoom = rooms[CrossSceneInformation.CurrentRoom];

        bool isCheckpoint = currentRoom.TryGetComponent(out CheckpointRoom checkpoint);
        if (isCheckpoint)
        {
            roomToActivate = currentRoom;
            checkpoint.RespawnSetDoors();
        }
        else if (CrossSceneInformation.CurrentRoom != 0) roomToActivate = rooms[CrossSceneInformation.CurrentRoom - 1];
        else if (startRoomGameObject != null) roomToActivate = rooms[rooms.IndexOf(startRoomGameObject) - 1];
        int roomsAtStart = isCheckpoint ? 1 : 2;

        ActivateRoom(roomToActivate, roomsAtStart);
    }
    private void TeleportToLastRoom()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<NavMeshAgent>().enabled = false;
        player.transform.position = currentRoom.GetComponent<RoomManager>().respawnPoint.position;
        player.GetComponent<NavMeshAgent>().enabled = true;
    }
    private void ActivateRoom(GameObject room, int activeRooms = 1)
    {
        int roomToActivate = rooms.IndexOf(room);
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].SetActive(i >= roomToActivate && i < roomToActivate + activeRooms);
        }
    }
    #endregion
    #region End Level
    public void EndLevel()
    {
        if (elapsedTime < GameManager.Instance.levels[levelId].highscore || GameManager.Instance.levels[levelId].highscore == 0)
            GameManager.Instance.levels[levelId].highscore = elapsedTime;
        GameManager.Instance.levels[levelId].unlocked = true;
        CrossSceneInformation.CurrentRoom = 0;
        CrossSceneInformation.CurrentTimerValue = 0;
        CrossSceneInformation.CurrentCheckpoint = 0;
        PlayerInputHandler.Instance.LockInputs();
        OnLevelEnd?.Invoke();
    }
    #endregion
}