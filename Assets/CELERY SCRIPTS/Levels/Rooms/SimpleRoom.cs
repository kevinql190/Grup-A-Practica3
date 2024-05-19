using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

[RequireComponent(typeof(WaveManager))]
public class SimpleRoom : RoomManager
{
    [Header("Simple Room")]
    [SerializeField] CinemachineVirtualCamera startRoomCamera;
    [SerializeField] private float startRoomTime;
    private WaveManager waveManager;
    [Header("Traps")]
    [SerializeField] List<GameObject> trapToDeactivate;
    override protected void Awake()
    {
        base.Awake();
        waveManager = GetComponent<WaveManager>();
    }
    protected override IEnumerator RoomSequence()
    {
        float defaultTime = waveManager.startWaveDelay;
        waveManager.startWaveDelay = startRoomTime;
        waveManager.enabled = true;
        if (startRoomCamera != null && !LevelManager.Instance.isDebugging) yield return StartCoroutine(ChangeCameraStart());
        waveManager.startWaveDelay = defaultTime;
        while (!waveManager.waveEnded) yield return null;
        EndRoom();
    }

    private IEnumerator ChangeCameraStart()
    {
        startRoomCamera.enabled = true;
        yield return new WaitForSecondsRealtime(startRoomTime/2f);
        startRoomCamera.enabled = false;
    }

    public override void EndRoom()
    {
        base.EndRoom();
        ChangeCameraEnd();
        DeactivateTraps();
    }
    private void ChangeCameraEnd()
    {
        foreach (StartDoor door in transform.GetComponentsInChildren<StartDoor>())
        {
            door.ChangeCamera();
        }
    }
    private void DeactivateTraps()
    {
        foreach (GameObject gObj in trapToDeactivate)
        {
            if (gObj.TryGetComponent(out ITrap trap))
                trap.Deactivate();
        }
    }
}
