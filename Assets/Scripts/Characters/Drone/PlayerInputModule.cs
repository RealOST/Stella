using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Characters.Drone {
    public class PlayerInputModule : ScriptableObject, InputActions.IGameplayActions {
        public event UnityAction<Vector2> onMove = delegate { };
        public event UnityAction onStopMove = delegate { };

        // 输入行为资产引用
        private InputActions inputActions;

        private void Awake()
        {
            inputActions = new InputActions();
        }
        private void OnEnable() => inputActions.Enable();
        private void OnDisable() => inputActions.Disable();
        
        public void DisableAllInputs() => inputActions.Disable();
        
        public void OnMove(InputAction.CallbackContext context) {
            throw new System.NotImplementedException();
        }

        public void OnFire(InputAction.CallbackContext context) {
            throw new System.NotImplementedException();
        }

        public void OnDodge(InputAction.CallbackContext context) {
            throw new System.NotImplementedException();
        }

        public void OnOverdrive(InputAction.CallbackContext context) {
            throw new System.NotImplementedException();
        }

        public void OnPause(InputAction.CallbackContext context) {
            throw new System.NotImplementedException();
        }

        public void OnLaunchMissile(InputAction.CallbackContext context) {
            throw new System.NotImplementedException();
        }

        public void OnTabSwitch(InputAction.CallbackContext context) {
            throw new System.NotImplementedException();
        }

        public void OnExit(InputAction.CallbackContext context) {
            throw new System.NotImplementedException();
        }
    }
}