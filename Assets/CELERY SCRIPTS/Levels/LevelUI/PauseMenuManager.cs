using UnityEngine.InputSystem;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public bool isPaused = false;
    private float lastTimeScale = 1;
    [SerializeField] Sprite cursorSprite;

    public void SetPause(bool willPause)
    {
        isPaused = willPause;
        if (willPause)
        {
            Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
            PlayerInputHandler.Instance.LockInputs();
            lastTimeScale = Time.timeScale;
        }
        else
            PlayerInputHandler.Instance.EnableInputs();
        PlayerInputHandler.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap(willPause ? "UI" : "Gameplay");
        AudioListener.pause = willPause;
        GameManager.Instance.ChangeTimeScale(willPause ? 0 : lastTimeScale);
    }
}
