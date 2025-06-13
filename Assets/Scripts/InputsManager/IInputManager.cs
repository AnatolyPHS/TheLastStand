using System;
using UnityEngine.InputSystem;

namespace InputsManager
{
    public enum InputType
    {
        None = 0,
        BuildClick = 10,
        UpgradeClick = 20,
        MeteorsAbility = 30,
        FreezingArrowAbility = 40,
        AllyBuildingSelected = 50,
    }

    public interface IInputManager
    {
        InputAction GetInputAction(string actionKey);
        bool IsPointerOverGameObject();
        void SubscribeToInputEvent(InputType type, Action<float> action);
        void UnsubscribeFromInputEvent(InputType type, Action<float> action);
        void RaiseInputEvent(InputType type, float value);
    }
}
