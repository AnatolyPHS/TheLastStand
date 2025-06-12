using UnityEngine;

namespace PoolingSystem
{
    public interface IPoolManager 
    {
        T GetObject<T>(T prefab, Vector3 spawnPointPosition, Quaternion spawnPointRotation) where T : MonoBehaviour;
        void ReturnObject(MonoBehaviour obj);
    }
}
