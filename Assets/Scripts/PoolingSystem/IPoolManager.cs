using UnityEngine;

namespace PoolingSystem
{
    public interface IPoolManager 
    {
        T GetObject<T>(T poolable) where T : MonoBehaviour;
        void ReturnObject(MonoBehaviour obj);
    }
}
