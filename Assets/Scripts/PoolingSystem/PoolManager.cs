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
            string poolId = prefab.gameObject.name;

            if (!pools.TryGetValue(poolId, out Pool pool))
            {
                pool = new Pool { prefab = prefab };
                pools[poolId] = pool;
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

            pooledObjectOnScene[obj.gameObject] = poolId;
            
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
            
            string poolID = pooledObjectOnScene.ContainsKey(obj.gameObject) ?
                pooledObjectOnScene[obj.gameObject] : obj.gameObject.name;

            if (pools.TryGetValue(poolID, out Pool pool) == false)
            {
                pool = new Pool {prefab = obj};
                pools[poolID] = pool;
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
