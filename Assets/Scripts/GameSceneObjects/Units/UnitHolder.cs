using System.Collections.Generic;
using Services;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class UnitHolder : MonoBehaviour, IUnitHolder
    {
        [SerializeField] private List<EnemyUnit> enemyUnits = new List<EnemyUnit>(); //TODO: need to optimize, mb via add sectors
        [SerializeField] private List<AllyUnit> allyUnits = new List<AllyUnit>();
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IUnitHolder>(this);
        }

        public bool TryGetGlosestEnemy(Vector3 currentPosition, out EnemyUnit unit)
        {
            unit = null;
            float closestDistanceSqrt = float.MaxValue;

            foreach (EnemyUnit enemyUnit in enemyUnits)
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
    }
}
