using System;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace InputsManager
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        public const string GameplayActionMapKey = "Gameplay";
        
        public const string MouseScreenPosActionKey = "MouseScreenPos";
        public const string LeftMouseClickActionKey = "LeftClick";
        public const string ToggleCameraFocesActionKey = "ToggleFocus";
        public const string ZoomCameraActionKey = "ZoomCamera";
        public static string RightMouseClickActionKey = "RightClick";
        
        private readonly Dictionary<string, InputAction> inputActions = new Dictionary<string, InputAction>();
        private readonly Dictionary<InputType, Input> inputs = new Dictionary<InputType, Input>();
        
        [SerializeField][Header("Input Actions")]
        private InputActionAsset playerInputActions;
        
        private bool pointerOverUI = false;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IInputManager>(this);
            
            InputActionMap gameplayActionMap = playerInputActions.FindActionMap(GameplayActionMapKey);

            inputActions.Add(MouseScreenPosActionKey, gameplayActionMap.FindAction(MouseScreenPosActionKey));
            inputActions.Add(LeftMouseClickActionKey, gameplayActionMap.FindAction(LeftMouseClickActionKey));
            inputActions.Add(ToggleCameraFocesActionKey, gameplayActionMap.FindAction(ToggleCameraFocesActionKey));
            inputActions.Add(ZoomCameraActionKey, gameplayActionMap.FindAction(ZoomCameraActionKey));
            inputActions.Add(RightMouseClickActionKey, gameplayActionMap.FindAction(RightMouseClickActionKey));
            
            EnableActions();
            SetEmptyInputs();
        }

        private void EnableActions()
        {
            foreach (InputAction action in inputActions.Values)
            {
                action.Enable();
            }
        }

        private void SetEmptyInputs()
        {
            foreach (InputType value in InputType.GetValues(typeof(InputType)))
            {
                inputs[value] = new Input();
            }
        }

        public InputAction GetInputAction(string actionKey)
        {
            if (inputActions.TryGetValue(actionKey, out InputAction action))
            {
                return action;
            }
            else
            {
                Debug.LogError($"[InputManager] Input action with key '{actionKey}' not found.");
                return null;
            }
        }

        public bool IsPointerOverGameObject()
        {
            return pointerOverUI;
        }

        public void SubscribeToInputEvent(InputType type, Action<float> action)
        {
            inputs[type].Subscribe(action);
        }

        public void UnsubscribeFromInputEvent(InputType type, Action<float> action)
        {
            inputs[type].Unsubscribe(action);
        }

        public void RaiseInputEvent(InputType type, float value)
        {
            inputs[type].Raise(value);
        }

        private void LateUpdate()
        {
            pointerOverUI = EventSystem.current.IsPointerOverGameObject();
        }
        
        private void OnDestroy()
        {
            ServiceLocator.Instance.Unregister<IInputManager>();
            
            foreach (InputAction action in inputActions.Values)
            {
                action.Disable();
            }
        }
    }
}
