using System;
using System.Collections.Generic;
using GameSceneObjects.Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameSceneObjects.Data
{
    [CreateAssetMenu(fileName = "SpawningBuildingInfo", menuName = "GameData/Spawning Building Info")]
    public class SpawningBuildingInfo : ScriptableObject
    {
        [SerializeField] private List<UnitToSpawnData> unitsToSpawn = new List<UnitToSpawnData>();
        
        public UnitToSpawnData GetUnitOfLevel(int level)
        {
            UnitToSpawnData result = null;
            
            for (int i = 0; i < unitsToSpawn.Count; i++)
            {
                if (unitsToSpawn[i].SpawnerLevel == level)
                {
                    result = unitsToSpawn[i];
                    break; 
                }
                
                if (unitsToSpawn[i].SpawnerLevel < level)
                {
                    result = unitsToSpawn[i];
                }    
            }
            
            return result;
        }
    }

    [Serializable]
    public class UnitToSpawnData
    {
        public int SpawnerLevel = 1;
        [FormerlySerializedAs("UnitToSpawn")] public GameUnit gameUnitToSpawn;
        public float SpawnDuration = 5f;
    }
}
