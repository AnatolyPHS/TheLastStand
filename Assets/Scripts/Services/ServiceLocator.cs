using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Instance => _instance;
        
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();
        
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            //TODO: add DontDestroyOnLoad(gameObject); if there will be more than one scene
        }
        
        public void Register<TService>(TService serviceInstance)
        {
            if (serviceInstance == null)
            {
                Debug.LogError($"[ServiceLocator] Attempted to register a null service for type {typeof(TService).Name}.");
                return;
            }

            Type serviceType = typeof(TService);
            if (services.ContainsKey(serviceType))
            {
                Debug.LogError($"[ServiceLocator] Service of type {serviceType.Name} already registered. Overwriting with new instance.");
                return;
            }

            services.Add(serviceType, serviceInstance);
        }
        
        public TService Get<TService>()
        {
            Type serviceType = typeof(TService);
            if (services.TryGetValue(serviceType, out object serviceInstance))
            {
                return (TService)serviceInstance;
            }
            else
            {
                Debug.LogError($"[ServiceLocator] Service of type {serviceType.Name} not found. Ensure it is registered.");
                return default;
            }
        }
        
        public void Unregister<TService>()
        {
            Type serviceType = typeof(TService);
            if (services.Remove(serviceType) == false)
            {
                Debug.LogError($"[ServiceLocator] Attempted to unregister service of type {serviceType.Name} which was not registered.");
            }
        }
    }
}
