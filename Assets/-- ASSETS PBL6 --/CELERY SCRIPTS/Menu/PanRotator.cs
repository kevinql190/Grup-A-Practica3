using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanRotator : MonoBehaviour
{
    [SerializeField] private GameObject pan;
    [SerializeField] private float smoothPanTime;
    private float _currentVelocity;
    private float targetangle;
    private void Start()
    {
        _currentVelocity = 0;
        targetangle = pan.transform.eulerAngles.y;
    }
    private void Update()
    {
        RotatePan();
    }
    #region Pan Rotation
    private void RotatePan()
    {
        float angle = Mathf.SmoothDampAngle(pan.transform.eulerAngles.y, targetangle, ref _currentVelocity, smoothPanTime);
        pan.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
    public void ChangePanRotation(int id)
    {
        targetangle = id;
    }
    #endregion
}
