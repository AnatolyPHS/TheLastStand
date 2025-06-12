using System;
using System.Collections.Generic;
using Services;
using UnityEngine;

namespace PoolingSystem
{
    public class PoolManager : MonoBehaviour, IPoolManager
    {
        private Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>();
        
        //TODO: Add preload of pooled objects on start
        
        public T GetObject<T>(T prefab, Vector3 spawnPos, Quaternion spawnRotation) where T : MonoBehaviour
        {
            Type type = prefab.GetType();

            if (!pools.TryGetValue(type, out Pool pool))
            {
                pool = new Pool { prefab = prefab };
                pools[type] = pool;
            }

            MonoBehaviour obj;
            if (pool.objects.Count > 0)
            {
                obj = pool.objects.Dequeue();
            }
            else
            {
                obj = Instantiate(pool.prefab);
            }

            obj.transform.position = spawnPos;
            obj.transform.rotation = spawnRotation;
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(null);

            if (obj is IPoolable ip)
            {
                ip.OnGetFromPool();    
            }
            
            return obj as T;
        }

        public void ReturnObject(MonoBehaviour obj)
        {   
            obj.gameObject.SetActive(false);

            obj.transform.SetParent(transform);
            if (obj is IPoolable poolable)
            {
                poolable.OnReturnToPool();
            }
            
            Type type = obj.GetType();

            if (pools.TryGetValue(type, out Pool pool) == false)
            {
                pool = new Pool {prefab = obj};
                pools[type] = pool;
                return;
            }
            pool.objects.Enqueue(obj);
        }

        private void Awake()
        {
            ServiceLocator.Instance.Register<IPoolManager>(this);
        }
        
        private class Pool
        {
            public Queue<MonoBehaviour> objects = new Queue<MonoBehaviour>();
            public MonoBehaviour prefab;
        }
    }
}
