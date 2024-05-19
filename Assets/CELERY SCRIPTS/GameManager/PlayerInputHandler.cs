using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerInputHandler : SingletonPersistent<PlayerInputHandler>
{
    [SerializeField] private InputActionAsset playerControls;
    public PlayerInput playerInput;

    private InputAction _moveAction;
    private InputAction _skillAimAction;
    private InputAction _attackAction;
    private InputAction _dashAction;
    private InputAction _pauseAction;
    private InputAction _pauseActionUI;

    public static Vector2 MoveInput { get; private set; }
    public static Vector2 SkillAimInput { get; private set; }
    public static bool AttackJustPressed { get; private set; }
    public static bool DashJustPressed { get; private set; }
    public static bool PauseJustPressed { get; private set; }
    public static string CurrentControlScheme { get; private set; }
    private void Update()
    {
        CurrentControlScheme = playerInput.currentControlScheme;
        AttackJustPressed = _attackAction.WasPressedThisFrame();
        DashJustPressed = _dashAction.WasPressedThisFrame();
        PauseJustPressed = _pauseAction.WasPressedThisFrame() || _pauseActionUI.WasPressedThisFrame();
    }
    public override void Awake()
    {
        base.Awake();
        _moveAction = playerControls.FindActionMap("Gameplay").FindAction("Move");
        _skillAimAction = playerControls.FindActionMap("Gameplay").FindAction("SkillAim");
        _attackAction = playerControls.FindActionMap("Gameplay").FindAction("Attack");
        _dashAction = playerControls.FindActionMap("Gameplay").FindAction("Dash");
        _pauseAction = playerControls.FindActionMap("Gameplay").FindAction("Pause");
        _pauseActionUI = playerControls.FindActionMap("UI").FindAction("SwitchActionMap");
        EnableInputs();
        RegisterInputActions();
    }
    private void RegisterInputActions()
    {
        _moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        _moveAction.canceled += context => MoveInput = Vector2.zero;

        _skillAimAction.performed += context => SkillAimInput = context.ReadValue<Vector2>();
        _skillAimAction.canceled += context => SkillAimInput = Vector2.zero;
    }
    public void EnableInputs()
    {
        _moveAction.Enable();
        _skillAimAction.Enable();
        _attackAction.Enable();
        _dashAction.Enable();
        _pauseAction.Enable();
        _pauseActionUI.Enable();
    }
    public void DisableInputs()
    {
        _moveAction.Disable();
        _skillAimAction.Disable();
        _attackAction.Disable();
        _dashAction.Disable();
        _pauseAction.Disable();
        _pauseActionUI.Disable();
        ResetInputs();
    }
    public void LockInputs()
    {
        _moveAction.Disable();
        _skillAimAction.Disable();
        _attackAction.Disable();
        _dashAction.Disable();
        ResetInputs();
    }
    private void ResetInputs()
    {
        MoveInput = Vector2.zero;
        SkillAimInput = Vector2.zero;
        AttackJustPressed = false;
        DashJustPressed = false;
    }
    public void OnlyMovement()
    {
        _pauseAction.Disable();
        _pauseActionUI.Disable();
        _skillAimAction.Disable();
        _attackAction.Disable();
        _dashAction.Disable();
    }
    public void UnlockInputs()
    {
        EnableInputs();
    }
}
