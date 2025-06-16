using Currencies;
using GameSceneObjects.Units;
using Selector;
using Services;
using UnityEngine;


namespace GameSceneObjects.Buildings
{
    public class AllySpawningBuilding : SpawningBuilding, IClickSelectable
    {
        [SerializeField] private GameObject SelectionMark;
        [SerializeField] private int upgradeCost = 100;

        private IBuildingManager buildingManager;
        private ICurrencyTracker currencyTracker;
        
        protected int unitsToSpawnNumber = 0;
        
        public void OnSelect()
        {
            SelectionMark.SetActive(true);
            buildingManager.OnSpawnerSelect(this);
        }

        public void OnDeselect()
        {
            buildingManager.OnSpawnerDeselect();
            SelectionMark.SetActive(false);
        }
        
        protected override void Start()
        {
            base.Start();
            
            buildingManager = ServiceLocator.Instance.Get<IBuildingManager>();
            currencyTracker = ServiceLocator.Instance.Get<ICurrencyTracker>();
        }
        
        protected override void OnSpawn(GameUnit gameUnit)
        {
            base.OnSpawn(gameUnit);
            unitsToSpawnNumber--;
        }

        protected override bool CanSpawn()
        {
            return unitsToSpawnNumber > 0;
        }

        public void UpgradeBuilding()
        {
            if (currencyTracker.CurrencyValue < upgradeCost)
            {
                return;
            }
            
            currencyTracker.ChangeCurrencyValue(-upgradeCost);
            currentBuildingLevel++;
        }

        public virtual void BuildUnit()
        {
            int price = nextUnit.gameUnitToSpawn.GetCost();
            if (currencyTracker.CurrencyValue < price)
            {
                return;
            }
            
            currencyTracker.ChangeCurrencyValue(-price);
            unitsToSpawnNumber++;
        }

        public float GetBuildingProgress()
        {
            if (unitsToSpawnNumber <= 0)
            {
                return 0f;
            }

            float progress = 1f - nextSpawnTimer / CalculateSpawnDuration();
            return Mathf.Clamp01(progress);
        }

        public void SetLevel(int currentWave)
        {
            throw new System.NotImplementedException();
        }

        public Sprite GetUnitIcon()
        {
            return nextUnit.gameUnitToSpawn.GetIcon();
        }

        public int GetUnitCost()
        {
            return nextUnit.gameUnitToSpawn.GetCost();
        }

        public int GetUpgradeCost()
        {
            return upgradeCost;
        }
    }
}
