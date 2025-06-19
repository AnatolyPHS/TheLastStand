using GameSceneObjects.Buildings;
using InputsManager;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameView
{
    public class BuildingView : View, IBuildingView
    {
        [SerializeField] private GameObject buildingPanel;
        [SerializeField] private Image buildingProgress;
        
        [SerializeField][Header("Spawner info")]
        private Image unitIcon;
        [SerializeField] private TextMeshProUGUI unitCost;
        [SerializeField] private TextMeshProUGUI upgradeCost;
        
        private IBuildingViewController buildingViewController;
        private IInputManager inputManager;
        
        public override void Init()
        {
            ServiceLocator.Instance.Register<IBuildingView>(this);
        }
        
        public override void OnMainUIStart()
        {
            inputManager = ServiceLocator.Instance.Get<IInputManager>();
            buildingViewController = ServiceLocator.Instance.Get<IBuildingViewController>();
            
            inputManager.SubscribeToInputEvent(InputType.AllyBuildingSelected, OnAllySpawnerSelected);
            inputManager.SubscribeToInputEvent(InputType.UpgradeClick, OnUpgradeClick);
        }
        
        public override void OnMainGuiDestroy()
        {
            ServiceLocator.Instance.Unregister<IBuildingView>();
            inputManager.UnsubscribeFromInputEvent(InputType.AllyBuildingSelected, OnAllySpawnerSelected);
            
            buildingPanel.SetActive(false);
            gameObject.SetActive(false);
        }

        private void OnUpgradeClick(float obj)
        {
            buildingViewController.OnUpgradeClick(obj);
            RefreshSelectedBuildingUI();
        }

        private void OnAllySpawnerSelected(float value)
        {
            bool isSelected = value > float.Epsilon;
            gameObject.SetActive(isSelected);
            buildingPanel.SetActive(isSelected);

            if (isSelected == false)
            {
                return;
            }
            
            RefreshSelectedBuildingUI();
        }

        private void RefreshSelectedBuildingUI()
        {
            AllySpawningBuilding allySpawner = buildingViewController.GetSelectedAllyBuilding();
            bool hasSelectedSpawner = allySpawner != null;
            
            unitIcon.sprite = hasSelectedSpawner ? allySpawner.GetUnitIcon() : null;
            unitCost.text = hasSelectedSpawner ? allySpawner.GetUnitCost().ToString() : string.Empty;
            upgradeCost.text = hasSelectedSpawner ? allySpawner.GetUpgradeCost().ToString() : string.Empty;
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
