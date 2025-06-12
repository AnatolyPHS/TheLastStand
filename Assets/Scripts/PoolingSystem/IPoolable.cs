using UnityEngine;

namespace PoolingSystem
{
    public interface IPoolable 
    {
        void OnReturnToPool();
        void OnGetFromPool();
    }
}
