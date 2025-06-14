using System;

namespace GameEvents
{
    public abstract class BaseEvent { }
    
    public interface IEventTrigger<TEvent, TParams> where TEvent : BaseEvent where TParams : struct
    {
        void AddListener(Action<TParams> listener);
        void RemoveListener(Action<TParams> listener);
    }
    
    
}