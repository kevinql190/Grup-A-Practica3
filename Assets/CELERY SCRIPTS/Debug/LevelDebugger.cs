using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDebugger : MonoBehaviour
{
    public void ChangeRoom(bool toNext)
    {
        int nextRoomNum = CrossSceneInformation.CurrentRoom + 2 * (toNext ? 1 : -1);
        if (nextRoomNum < 0) nextRoomNum = 0;
        else if (nextRoomNum > LevelManager.Instance.rooms.Count - 1) nextRoomNum = LevelManager.Instance.rooms.Count - 1;
        CrossSceneInformation.CurrentRoom = nextRoomNum;
        CrossSceneInformation.CurrentTimerValue = LevelManager.Instance.elapsedTime;
        AudioManager.Instance.StopAllLoops();
        AudioListener.pause = false;
        PlayerInputHandler.Instance.EnableInputs();
        GameManager.Instance.ChangeTimeScale(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
