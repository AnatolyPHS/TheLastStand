using UnityEngine;

namespace GameSceneObjects.Units
{
    public enum UnitFaction
    {
        None = 0,
        Enemy = 10,
        Ally = 20
    }
    
    public interface IUnitHolder
    {
        bool TryGetGlosestEnemy(Vector3 currentPosition, out EnemyUnit unit);
        void RegisterUnit(Unit unit);
        void UnregisterUnit(Unit unit);
        int GetUnitsCount(UnitFaction faction);
    }
}
