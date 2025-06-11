using UnityEngine;

namespace GameSceneObjects.Units
{
    public interface IUnitHolder
    {
        bool TryGetGlosestEnemy(Vector3 currentPosition, out EnemyUnit unit);
    }
}
