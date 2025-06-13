using GameSceneObjects.Buildings;
using InputsManager;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameView
{
    public class BuiildingView : View, IBuildingView
    {
        [SerializeField] private GameObject buildingPanel;
        [SerializeField] private Image buildingProgress;
        
        private IBuildingViewController buildingViewController;
        private IInputManager inputManager;
        
        public override void Init()
        {
            ServiceLocator.Instance.Register<IBuildingView>(this);
        }
        
        private void Start()
        {
            inputManager = ServiceLocator.Instance.Get<IInputManager>();
            buildingViewController = ServiceLocator.Instance.Get<IBuildingViewController>();
            
            inputManager.SubscribeToInputEvent(InputType.AllyBuildingSelected, OnAllySpawnerSelected);
        }

        private void OnAllySpawnerSelected(float value)
        {
            buildingPanel.SetActive(value > float.Epsilon);
        }

        private void Update()
        {
            if (buildingPanel.activeSelf == false)
            {
                return;
            }
            
            AllySpawningBuilding allySpawner = buildingViewController.GetSelectedAllyBuilding();
            buildingProgress.fillAmount = allySpawner.GetBuildingProgress();
        }
    }
}
