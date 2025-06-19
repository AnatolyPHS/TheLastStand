using Currencies;
using GameSceneObjects.Units;
using MainGameLogic;
using Selector;
using Services;
using UnityEngine;


namespace GameSceneObjects.Buildings
{
    public class AllySpawningBuilding : SpawningBuilding, IClickSelectable
    {
        [SerializeField] private GameObject SelectionMark;
        [SerializeField] private int upgradeCost = 100;
        [SerializeField] private AnimationCurve trainingSpeedCurve;

        private IBuildingManager buildingManager;
        private ICurrencyTracker currencyTracker;
        private IWavesSpawner wavesSpawner;
        
        protected int unitsToSpawnNumber = 0;
        private float trainingSpeedFactor = 1f;
        
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
            wavesSpawner = ServiceLocator.Instance.Get<IWavesSpawner>();
        }
        
        protected override void OnSpawn(GameUnit gameUnit)
        {
            base.OnSpawn(gameUnit);
            unitsToSpawnNumber--;
        }

        protected override bool CanSpawn()
        {
            return unitsToSpawnNumber > 0 && wavesSpawner.GameStarted;
        }

        public void UpgradeBuilding()
        {
            if (currencyTracker.CurrencyValue < GetUpgradeCost())
            {
                return;
            }
            
            float oldSpawnDuration = CalculatetSpawnDuration();
            currencyTracker.ChangeCurrencyValue(-GetUpgradeCost());
            currentBuildingLevel++;
            nextUnit = spawningBuildingInfo.GetUnitOfLevel(currentBuildingLevel);
            trainingSpeedFactor = trainingSpeedCurve.Evaluate(currentBuildingLevel);
            nextSpawnTimer = nextSpawnTimer * CalculatetSpawnDuration() / oldSpawnDuration;
        }

        public virtual void BuildUnit()
        {
            int price = nextUnit.UnitSpawnCost;
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

            float progress = 1f - nextSpawnTimer / CalculatetSpawnDuration();
            return Mathf.Clamp01(progress);
        }

        public void SetLevel(int currentWave)
        {
            currentBuildingLevel = currentWave;
        }

        public Sprite GetUnitIcon()
        {
            return nextUnit.gameUnitToSpawn.GetIcon();
        }

        public int GetUnitCost()
        {
            return nextUnit.UnitSpawnCost;
        }

        public int GetUpgradeCost()
        {
            return upgradeCost * currentBuildingLevel;
        }

        protected override void ReduceTimer()
        {
            nextSpawnTimer -= Time.deltaTime * trainingSpeedFactor;
        }
    }
}
