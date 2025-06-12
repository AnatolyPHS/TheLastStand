using System;
using System.Collections.Generic;
using GameSceneObjects.Units;
using UnityEngine;

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
                if (unitsToSpawn[i].Level == level)
                {
                    result = unitsToSpawn[i];
                    break; 
                }
                
                if (unitsToSpawn[i].Level < level)
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
        public int Level;
        public Unit Unit;
        public float SpawnDuration = 5f;
    }
}
