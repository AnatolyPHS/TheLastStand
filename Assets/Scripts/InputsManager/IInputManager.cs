using UnityEngine.InputSystem;

namespace InputsManager
{
    public interface IInputManager
    {
        InputAction GetInputAction(string actionKey);
        bool IsPointerOverGameObject();
    }
}
