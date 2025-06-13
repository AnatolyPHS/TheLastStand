using InputsManager;
using Services;
using UI.GameView;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class BuildingManager : MonoBehaviour, IBuildingManager
    {
        [SerializeField] GameObject mainTower;
        [SerializeField] GameObject sanctum;
        
        private IInputManager inputManager;
        private IGameView gameView;

        private AllySpawningBuilding allySpawner;
        
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
            this.allySpawner = allySpawer;
            gameView.SetBuildingPanelState(true);
        }

        public void OnSpawnerDeselect()
        {
            allySpawner = null;
            gameView.SetBuildingPanelState(false);
        }
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IBuildingManager>(this);
        }

        private void Start()
        {
            gameView = ServiceLocator.Instance.Get<IGameView>();
            inputManager = ServiceLocator.Instance.Get<IInputManager>();
            
            inputManager.SubscribeToInputEvent(InputType.BuildClick, OnBuildClick);
            inputManager.SubscribeToInputEvent(InputType.UpgradeClick, OnUpgradeClick);
        }

        private void OnUpgradeClick(float value)
        {
            if (value > 0f && allySpawner != null)
            {
                allySpawner.UpgradeBuilding();
            }
        }

        private void OnBuildClick(float value)
        {
            if (value > 0f)
            {
                allySpawner.BuildUnit();
            }
        }

        private void OnDestroy()
        {
            inputManager.UnsubscribeFromInputEvent(InputType.BuildClick, OnBuildClick);
            inputManager.UnsubscribeFromInputEvent(InputType.UpgradeClick, OnUpgradeClick);
            ServiceLocator.Instance.Unregister<IBuildingManager>();
        }
    }
}
