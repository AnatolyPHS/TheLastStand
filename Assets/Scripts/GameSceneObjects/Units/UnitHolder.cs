using System.Collections.Generic;
using Services;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class UnitHolder : MonoBehaviour, IUnitHolder
    {
        private Dictionary<UnitFaction, List<Unit>> unitsByFaction = new Dictionary<UnitFaction, List<Unit>>()
        {
            {UnitFaction.Ally, new List<Unit>()},
            {UnitFaction.Enemy, new List<Unit>()}
        };
        
        public bool TryGetGlosestEnemy(Vector3 currentPosition, out EnemyUnit unit)
        {
            unit = null;
            float closestDistanceSqrt = float.MaxValue;
            List<Unit> enemyUnits = unitsByFaction[UnitFaction.Enemy];
            foreach (EnemyUnit enemyUnit in enemyUnits) //TODO: optimize, use spatial partitioning or similar
            {
                if (enemyUnit == null || !enemyUnit.gameObject.activeInHierarchy)
                {
                    continue;
                }

                float distanceSqrt = (enemyUnit.transform.position - currentPosition).sqrMagnitude;
                if (distanceSqrt < closestDistanceSqrt)
                {
                    closestDistanceSqrt = distanceSqrt;
                    unit = enemyUnit;
                }
            }

            return enemyUnits.Count > 0;
        }

        public void RegisterUnit(Unit unit)
        {
            if (unitsByFaction.TryGetValue(unit.GetFaction(), out List<Unit> units) == false)
            {
                units = new List<Unit>();
                unitsByFaction[unit.GetFaction()] = units;
            }

            if (units.Contains(unit) == false)
            {
                units.Add(unit);
            }
        }
        
        public void UnregisterUnit(Unit unit)
        {
            if (unitsByFaction.TryGetValue(unit.GetFaction(), out List<Unit> units))
            {
                units.Remove(unit);
            }
        }
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IUnitHolder>(this);
        }
        
        private
    }
}
