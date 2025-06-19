using GameSceneObjects.Data;
using GameSceneObjects.Units;
using PoolingSystem;
using Services;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class SpawningBuilding : BuildingBase
    {
        [SerializeField] protected SpawningBuildingInfo spawningBuildingInfo;
        [SerializeField] protected Transform spawnPoint;

        protected IUnitHolder unitsHolder;
        protected IPoolManager poolManager;

        protected int currentBuildingLevel = 1;

        protected UnitToSpawnData nextUnit;
        protected float nextSpawnTimer = float.MinValue;
        
        protected virtual void Start()
        {
            unitsHolder = ServiceLocator.Instance.Get<IUnitHolder>();
            poolManager = ServiceLocator.Instance.Get<IPoolManager>();
            
            nextUnit = spawningBuildingInfo.GetUnitOfLevel(currentBuildingLevel);
            SetNextSpawnTimer();
        }
        
        protected virtual bool CanSpawn()
        {
            return true;
        }
        
        protected virtual void Update()
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
                ReduceTimer();
                return;
            }

            nextUnit = spawningBuildingInfo.GetUnitOfLevel(currentBuildingLevel);

            GameUnit gameUnit = poolManager.GetObject(nextUnit.gameUnitToSpawn, spawnPoint.position, spawnPoint.rotation);
            
            OnSpawn(gameUnit);

            SetNextSpawnTimer();
        }

        protected virtual void ReduceTimer()
        {
            nextSpawnTimer -= Time.deltaTime;
        }

        protected float GetSpawnDuration()
        {
            return nextUnit.SpawnDuration;
        }

        protected virtual void OnSpawn(GameUnit gameUnit)
        {
            gameUnit.SetLevel(nextUnit.SpawnerLevel);
            gameUnit.Init();
            unitsHolder.RegisterUnit(gameUnit);
        }
        
        private void SetNextSpawnTimer()
        {
            nextSpawnTimer = nextUnit.SpawnDuration;
        }
    }
}
