using GameSceneObjects.Data;
using GameSceneObjects.Units;
using PoolingSystem;
using Selector;
using Services;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class SpawningBuilding : BuildingBase
    {
        [SerializeField] private SpawningBuildingInfo spawningBuildingInfo;
        [SerializeField] private Transform spawnPoint;
        
        private IUnitHolder unitsHolder;
        private IPoolManager poolManager;

        private int currentBuildingLevel = 1;

        private float nextSpawnTime = float.MinValue;
        
        protected virtual void Start()
        {
            unitsHolder = ServiceLocator.Instance.Get<IUnitHolder>();
            poolManager = ServiceLocator.Instance.Get<IPoolManager>();
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
            if (Time.time < nextSpawnTime)
            {
                return;
            }

            UnitToSpawnData unitToSpawnData = spawningBuildingInfo.GetUnitOfLevel(currentBuildingLevel);

            Unit unit = poolManager.GetObject(unitToSpawnData.Unit);
            unit.Init();
            unit.transform.position = spawnPoint.position;
            unit.transform.rotation = spawnPoint.rotation;

            OnSpawn(unit);

            nextSpawnTime = Time.time + unitToSpawnData.SpawnDuration;
        }

        protected virtual void OnSpawn(Unit unit)
        {
            unitsHolder.RegisterUnit(unit);
        }
    }
}
