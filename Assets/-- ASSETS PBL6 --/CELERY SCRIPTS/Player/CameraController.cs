using UnityEngine;
using System.Collections;
using Cinemachine;
using System;

public class CameraController : MonoBehaviour
{    
    public float _normalZoffset;
    [SerializeField] private float smoothInZ;
    [SerializeField] private float smoothOutZ;
    [SerializeField] private float dashOffsetZ;

    private PlayerMovement PlayerMovement => GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    private float targetOffset;

    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _cameraNoise;
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _normalZoffset = _virtualCamera.m_Lens.OrthographicSize;
        targetOffset = _normalZoffset;
        _virtualCamera.Follow = PlayerMovement.transform;
        _cameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void OnEnable()
    {
        LevelManager.Instance.OnStartLevel += ctx => StartLevel(ctx);
        PlayerMovement.OnDashStart += PlayerMovement_OnDashStart;
    }
    private void PlayerMovement_OnDashStart(bool isDashing)
    {
        targetOffset = isDashing ? _normalZoffset + dashOffsetZ : _normalZoffset;
    }
    private void FixedUpdate()
    {
        _virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothStep(_virtualCamera.m_Lens.OrthographicSize, targetOffset, smoothInZ * Time.deltaTime);
    }
    public void CameraShake(float intensity, float duration)
    {
        StartCoroutine(DoCameraShake(intensity, duration));
    }
    IEnumerator DoCameraShake(float intensity, float duration)
    {
        _cameraNoise.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(duration);
        _cameraNoise.m_AmplitudeGain = 0;
    }
    #region Ortho Size
    private void StartLevel(bool ctx)
    {
        LevelManager.Instance.OnStartLevel -= ctx => StartLevel(ctx);
        if (ctx) StartCoroutine(StartLevelCamera());
    }
    private IEnumerator StartLevelCamera()
    {
        PlayerInputHandler.Instance.DisableInputs();
        GameManager.Instance.ChangeTimeScale(0);
        float t = 0;
        while (t < LevelManager.Instance.startTime)
        {
            t += Time.unscaledDeltaTime;
            float value = t / LevelManager.Instance.startTime;
            GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = Mathf.SmoothStep(LevelManager.Instance.startOrthoSize, _normalZoffset, value);
            yield return null;
        }
        GameManager.Instance.ChangeTimeScale(1);
        PlayerInputHandler.Instance.EnableInputs();
    }
    public IEnumerator ChangeOrthoSize(float endValue, float lerpTime = 0)
    {
        float startOrthSize = _virtualCamera.m_Lens.OrthographicSize;
        float t = 0;
        while (t < lerpTime)
        {
            t += Time.unscaledDeltaTime;
            float value = t / lerpTime;
            _virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothStep(startOrthSize, endValue, value);
            yield return null;
        }
    }
    #endregion
}
