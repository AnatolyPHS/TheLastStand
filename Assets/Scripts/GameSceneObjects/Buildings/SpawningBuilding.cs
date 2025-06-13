using GameSceneObjects.Data;
using GameSceneObjects.Units;
using PoolingSystem;
using Services;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class SpawningBuilding : BuildingBase
    {
        [SerializeField] private SpawningBuildingInfo spawningBuildingInfo;
        [SerializeField] private Transform spawnPoint;

        protected IUnitHolder unitsHolder;
        private IPoolManager poolManager;

        protected int currentBuildingLevel = 1;

        protected UnitToSpawnData nextUnit;
        protected float nextSpawnTimer = float.MinValue;
        
        protected virtual void Start()
        {
            unitsHolder = ServiceLocator.Instance.Get<IUnitHolder>();
            poolManager = ServiceLocator.Instance.Get<IPoolManager>();
            
            nextUnit = spawningBuildingInfo.GetUnitOfLevel(currentBuildingLevel);
            nextSpawnTimer = nextUnit.SpawnDuration;
        }

        protected virtual bool CanSpawn()
        {
            return true;
        }
        
        private void Update()
        {
            if (CanSpawn() == false)
            {
                return;
            }
            
            ProcessSpawning();
        }

        private void ProcessSpawning()
        {
            if (nextSpawnTimer > 0)
            {
                nextSpawnTimer -= Time.deltaTime;
                return;
            }

            nextUnit = spawningBuildingInfo.GetUnitOfLevel(currentBuildingLevel);

            Unit unit = poolManager.GetObject(nextUnit.Unit, spawnPoint.position, spawnPoint.rotation);
            unit.Init();

            OnSpawn(unit);

            nextSpawnTimer = nextUnit.SpawnDuration;
        }

        protected virtual void OnSpawn(Unit unit)
        {
            unitsHolder.RegisterUnit(unit);
        }
    }
}
