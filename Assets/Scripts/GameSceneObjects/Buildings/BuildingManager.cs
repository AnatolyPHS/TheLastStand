using Services;
using UI.GameView;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class BuildingManager : MonoBehaviour, IBuildingManager
    {
        [SerializeField] GameObject mainTower;
        [SerializeField] GameObject sanctum;
        
        private IGameView gameView;
        
        public Vector3 GetMainTowerPosition()
        {
            return mainTower.transform.position;
        }

        public Vector3 GetSanctumPosition()
        {
            return sanctum.transform.position;
        }

        public void OnSpawnerSelect(AllySpawningBuilding allySpawningBuilding)
        {
            gameView.SetBuildingPanelState(true);
        }

        public void OnSpawnerDeselect(AllySpawningBuilding allySpawningBuilding)
        {
            gameView.SetBuildingPanelState(false);
        }
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IBuildingManager>(this);
        }

        private void Start()
        {
            gameView = ServiceLocator.Instance.Get<IGameView>();
        }
    }
}
