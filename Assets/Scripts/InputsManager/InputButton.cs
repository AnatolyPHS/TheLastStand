using Services;
using UnityEngine;

namespace InputsManager
{
    public class InputButton : MonoBehaviour
    {
        [SerializeField] protected InputType inputType;
        
        private IInputManager inputManager;
        
        public void OnButtonClicked()
        {
            inputManager.RaiseInputEvent(inputType, 1f);
        }
        
        private void Start()
        {
            inputManager = ServiceLocator.Instance.Get<IInputManager>();
        }
        
    }
}
