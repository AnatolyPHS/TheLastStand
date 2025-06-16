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
        bool TryGetGlosestUnit(UnitFaction enemyFaction, Vector3 currentPosition, out GameUnit gameUnit);
        void RegisterUnit(GameUnit gameUnit);
        void UnregisterUnit(GameUnit gameUnit);
        int GetUnitsCount(UnitFaction faction);
        bool HasEnemiesOnField();
    }
}
