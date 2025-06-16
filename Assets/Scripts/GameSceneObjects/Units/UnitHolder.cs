using System.Collections.Generic;
using Services;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class UnitHolder : MonoBehaviour, IUnitHolder
    {
        private Dictionary<UnitFaction, List<GameUnit>> unitsByFaction = new Dictionary<UnitFaction, List<GameUnit>>()
        {
            {UnitFaction.None, new List<GameUnit>()},
            {UnitFaction.Ally, new List<GameUnit>()},
            {UnitFaction.Enemy, new List<GameUnit>()}
        };
        
        public bool TryGetGlosestUnit(UnitFaction enemyFaction, Vector3 currentPosition, out EnemyGameUnit gameUnit)
        {
            gameUnit = null;
            float closestDistanceSqrt = float.MaxValue;
            List<GameUnit> enemyUnits = unitsByFaction[enemyFaction];
            foreach (EnemyGameUnit enemyUnit in enemyUnits) //TODO: optimize, use spatial partitioning or similar
            {
                if (enemyUnit == null || !enemyUnit.gameObject.activeInHierarchy)
                {
                    continue;
                }

                float distanceSqrt = (enemyUnit.transform.position - currentPosition).sqrMagnitude;
                if (distanceSqrt < closestDistanceSqrt)
                {
                    closestDistanceSqrt = distanceSqrt;
                    gameUnit = enemyUnit;
                }
            }

            return enemyUnits.Count > 0;
        }

        public void RegisterUnit(GameUnit gameUnit)
        {
            if (unitsByFaction.TryGetValue(gameUnit.GetFaction(), out List<GameUnit> units) == false)
            {
                units = new List<GameUnit>();
                unitsByFaction[gameUnit.GetFaction()] = units;
            }

            if (units.Contains(gameUnit) == false)
            {
                units.Add(gameUnit);
            }
        }
        
        public void UnregisterUnit(GameUnit gameUnit)
        {
            if (unitsByFaction.TryGetValue(gameUnit.GetFaction(), out List<GameUnit> units))
            {
                units.Remove(gameUnit);
            }
        }

        public int GetUnitsCount(UnitFaction faction)
        {
            return unitsByFaction[faction].Count;
        }

        public bool HasEnemiesOnField()
        {
            return unitsByFaction[UnitFaction.Enemy].Count > 0;
        }

        private void Awake()
        {
            ServiceLocator.Instance.Register<IUnitHolder>(this);
        }
    }
}
