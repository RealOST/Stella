using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// 通过CreateAssetMenu特性支持编辑器资产创建
[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions, InputActions.IPauseMenuActions,
    InputActions.IGameOverScreenActions {
    // 事件驱动接口
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStopMove = delegate { };
    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };
    public event UnityAction onDodge = delegate { };
    public event UnityAction onOverdrive = delegate { };
    public event UnityAction onPause = delegate { };
    public event UnityAction onUnpause = delegate { };
    public event UnityAction onConfirmGameOver = delegate { };
    public event UnityAction onLaunchMissile = delegate { };
    public event UnityAction<float> onTabSwitch = delegate { };
    public event UnityAction onExit = delegate { };

    // 输入行为资产引用
    private InputActions inputActions;

    private void OnEnable() {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }

    private void OnDisable() {
        DisableAllInputs();
    }
    
    public void DisableAllInputs() => inputActions.Disable();

    private void SwitchActionMap(InputActionMap actionMap, bool isUIInput) {
        inputActions.Disable();
        actionMap.Enable();

        if (isUIInput) {
            // Cursor.visible = true;
            // Cursor.lockState = CursorLockMode.None;
            SwitchToDynamicUpdateMode();
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            SwitchToFixedUpdateMode();
        }
    }

    public void SwitchToDynamicUpdateMode() =>
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    public void SwitchToFixedUpdateMode() =>
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void EnableGameplayInput() => SwitchActionMap(inputActions.Gameplay, false);

    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);

    public void EnableGameOverScreenInput() => SwitchActionMap(inputActions.GameOverScreen, true);

    public void OnMove(InputAction.CallbackContext context) {
        if (context.performed) onMove.Invoke(context.ReadValue<Vector2>());

        if (context.canceled) onStopMove.Invoke();
    }

    public void OnFire(InputAction.CallbackContext context) {
        if (context.performed) onFire.Invoke();

        if (context.canceled) onStopFire.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context) {
        if (context.performed) onDodge.Invoke();
    }

    public void OnOverdrive(InputAction.CallbackContext context) {
        if (context.performed) onOverdrive.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context) {
        if (context.performed) onPause.Invoke();
    }

    public void OnLaunchMissile(InputAction.CallbackContext context) {
        if (context.performed) onLaunchMissile.Invoke();
    }

    public void OnTabSwitch(InputAction.CallbackContext context) {
        if (context.performed) onTabSwitch.Invoke(context.ReadValue<float>());
    }

    public void OnExit(InputAction.CallbackContext context) {
        if (context.performed) onExit.Invoke();
    }

    public void OnUnpause(InputAction.CallbackContext context) {
        if (context.performed) onUnpause.Invoke();
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context) {
        if (context.performed) onConfirmGameOver.Invoke();
    }
}