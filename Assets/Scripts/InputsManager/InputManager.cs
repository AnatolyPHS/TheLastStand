using System;
using System.Collections.Generic;
using Services;
using UnityEngine;
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
        
        [SerializeField][Header("Input Actions")]
        private InputActionAsset playerInputActions;
        
        private readonly Dictionary<string, InputAction> inputActions = new Dictionary<string, InputAction>();
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IInputManager>(this);
            
            InputActionMap gameplayActionMap = playerInputActions.FindActionMap(GameplayActionMapKey);

            inputActions.Add(MouseScreenPosActionKey, gameplayActionMap.FindAction(MouseScreenPosActionKey));
            inputActions.Add(LeftMouseClickActionKey, gameplayActionMap.FindAction(LeftMouseClickActionKey));
            inputActions.Add(ToggleCameraFocesActionKey, gameplayActionMap.FindAction(ToggleCameraFocesActionKey));
            inputActions.Add(ZoomCameraActionKey, gameplayActionMap.FindAction(ZoomCameraActionKey));
            inputActions.Add(RightMouseClickActionKey, gameplayActionMap.FindAction(RightMouseClickActionKey));
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
    }
}
