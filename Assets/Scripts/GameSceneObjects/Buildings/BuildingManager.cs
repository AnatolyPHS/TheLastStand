using InputsManager;
using Services;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class BuildingManager : MonoBehaviour, IBuildingManager
    {
        [SerializeField] GameObject mainTower;
        [SerializeField] GameObject sanctum;
        
        private IBuildingViewController buildingViewController;
        
        public Vector3 GetMainTowerPosition()
        {
            return mainTower.transform.position;
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
