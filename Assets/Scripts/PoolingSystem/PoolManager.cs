using System.Collections.Generic;
using Services;
using UnityEngine;

namespace PoolingSystem
{
    public class PoolManager : MonoBehaviour, IPoolManager
    {
        private readonly Dictionary<string, Pool> pools = new Dictionary<string, Pool>();
        private readonly Dictionary<GameObject, string> pooledObjectOnScene = new Dictionary<GameObject, string>();
        
        //TODO: Add preload of pooled objects on start
        
        public T GetObject<T>(T prefab, Vector3 spawnPos, Quaternion spawnRotation) where T : MonoBehaviour
        {
            string type = prefab.gameObject.name;

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

            pooledObjectOnScene[obj.gameObject] = type;
            
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
            
            string type = pooledObjectOnScene[obj.gameObject];

            if (pools.TryGetValue(type, out Pool pool) == false)
            {
                pool = new Pool {prefab = obj};
                pools[type] = pool;
            }
            
            pooledObjectOnScene.Remove(obj.gameObject);
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
