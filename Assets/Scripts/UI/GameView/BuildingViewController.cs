using GameSceneObjects.Buildings;
using InputsManager;
using Services;
using UnityEngine;

namespace UI.GameView
{
    public class BuildingViewController : MonoBehaviour, IBuildingViewController
    {
        private IInputManager inputManager;
        
        private AllySpawningBuilding allySpawner;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IBuildingViewController>(this);
        }

        private void Start()
        {
            inputManager = ServiceLocator.Instance.Get<IInputManager>();
            
            inputManager.SubscribeToInputEvent(InputType.BuildClick, OnBuildClick);
        }
        
        public void SetSelectedAllyBuilding(AllySpawningBuilding building)
        {
            allySpawner = building;
            inputManager.RaiseInputEvent(InputType.AllyBuildingSelected, allySpawner == null ? 0f : 1f);
        }

        public AllySpawningBuilding GetSelectedAllyBuilding()
        {
            return allySpawner;
        }
        
        public void OnUpgradeClick(float value)
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
            
            ServiceLocator.Instance.Unregister<IBuildingViewController>();
        }
    }
}
