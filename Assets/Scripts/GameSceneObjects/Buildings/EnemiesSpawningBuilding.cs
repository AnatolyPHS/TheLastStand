using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class EnemiesSpawningBuilding : SpawningBuilding
    {
        [SerializeField] private AnimationCurve enemiesNumberALevel;
        
        private int spawnedEnemiesCount = 0;
        private int needToSpawnCount = 0;
        
        protected override bool CanSpawn()
        {
            return spawnedEnemiesCount < needToSpawnCount;
        }

        public void StartNextWaveSpawn(int currentWave)
        {
            currentBuildingLevel = currentWave;
            needToSpawnCount = Mathf.RoundToInt(enemiesNumberALevel.Evaluate(currentBuildingLevel));
            nextSpawnTimer = float.MinValue;
        }
        
        protected override void OnSpawn(GameUnit gameUnit)
        {
            base.OnSpawn(gameUnit);
            spawnedEnemiesCount++;
        }

        public bool FinishedSpawn()
        {
            return spawnedEnemiesCount >= needToSpawnCount;
        }
    }
}
