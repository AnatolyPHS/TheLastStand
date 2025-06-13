using System;

namespace InputsManager
{
    public class Input
    {
        private Action<float> actions;
        
        public void Raise(float value)
        {
            actions?.Invoke(value);
        }

        public void Subscribe(Action<float> action)
        {
            actions += action;
        }

        public void Unsubscribe(Action<float> action)
        {
            if (action != null && actions != null)
            {
                actions -= action;
            }
        }
    }
}
