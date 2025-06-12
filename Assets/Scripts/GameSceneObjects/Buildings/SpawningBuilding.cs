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

        private float nextSpawnTimer = float.MinValue;
        
        protected virtual void Start()
        {
            unitsHolder = ServiceLocator.Instance.Get<IUnitHolder>();
            poolManager = ServiceLocator.Instance.Get<IPoolManager>();
            
            nextSpawnTimer = spawningBuildingInfo.GetUnitOfLevel(currentBuildingLevel).SpawnDuration;
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

            UnitToSpawnData unitToSpawnData = spawningBuildingInfo.GetUnitOfLevel(currentBuildingLevel);

            Unit unit = poolManager.GetObject(unitToSpawnData.Unit, spawnPoint.position, spawnPoint.rotation);
            unit.Init();

            OnSpawn(unit);

            nextSpawnTimer = unitToSpawnData.SpawnDuration;
        }

        protected virtual void OnSpawn(Unit unit)
        {
            unitsHolder.RegisterUnit(unit);
        }
    }
}
