using System.Collections.Generic;
using Services;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class BuildingManager : MonoBehaviour, IBuildingManager
    {
        [SerializeField] Сitadel citadel;
        [SerializeField] Sanctum sanctum;
        
        [SerializeField][Header("Enemy barraks")]
        private List<EnemiesSpawningBuilding> allySpawners = new List<EnemiesSpawningBuilding>();
        
        private IBuildingViewController buildingViewController;
        
        public Vector3 GetMainTowerPosition()
        {
            return citadel.transform.position;
        }

        public Vector3 GetSanctumPosition()
        {
            return sanctum.transform.position;
        }

        public void OnSpawnerSelect(AllySpawningBuilding allySpawer)
        {
            buildingViewController.SetSelectedAllyBuilding(allySpawer);
        }

        public void OnSpawnerDeselect()
        {
            buildingViewController.SetSelectedAllyBuilding(null);
        }

        public Sanctum GetSanctum()
        {
            return sanctum;
        }

        public void SetEnemySpawnersLevel(int currentWave)
        {
            foreach (var spawner in allySpawners)
            {
                spawner.StartNextWaveSpawn(currentWave);
            }
        }

        public bool EnemiesAreSpawning()
        {
            foreach (var spawner in allySpawners)
            {
                if (spawner.FinishedSpawn() == false)
                {
                    return true;
                }
            }
            
            return false;
        }

        public Сitadel GetCitadel()
        {
            return citadel;
        }

        private void Awake()
        {
            ServiceLocator.Instance.Register<IBuildingManager>(this);
        }

        private void Start()
        {
            buildingViewController = ServiceLocator.Instance.Get<IBuildingViewController>();
        }
        
        private void OnDestroy()
        {
            ServiceLocator.Instance.Unregister<IBuildingManager>();
        }
    }
}
