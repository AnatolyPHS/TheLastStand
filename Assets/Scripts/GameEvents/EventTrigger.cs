using System;
using System.Collections.Generic;

namespace GameEvents
{
    public class EventTrigger
    {
        private readonly Dictionary<Type, Delegate> eventTable = new();
        
        public void AddListener<TEvent, TParams>(Action<TParams> listener) where TEvent : BaseEvent where TParams : struct
        {
            Type key = typeof(TEvent);
            if (eventTable.TryGetValue(key, out var del))
            {
                eventTable[key] = Delegate.Combine(del, listener);
            }
            else
            {
                eventTable[key] = listener;
            }
        }
        
        public void RemoveListener<TEvent, TParams>(Action<TParams> listener) where TEvent : BaseEvent where TParams : struct
        {
            Type key = typeof(TEvent);
            if (eventTable.TryGetValue(key, out var del))
            {
                var newDel = Delegate.Remove(del, listener);
                if (newDel == null)
                    eventTable.Remove(key);
                else
                    eventTable[key] = newDel;
            }
        }
        
        public void RaiseEvent<TEvent, TParams>(TParams parameters) where TEvent : BaseEvent where TParams : struct
        {
            Type key = typeof(TEvent);
            if (eventTable.TryGetValue(key, out var del))
            {
                if (del is Action<TParams> callback)
                {
                    callback.Invoke(parameters);
                }
            }
        }
    }
}
