using GameSceneObjects.Units;
using Selector;
using Services;
using UnityEngine;


namespace GameSceneObjects.Buildings
{
    public class AllySpawningBuilding : SpawningBuilding, IClickSelectable
    {
        [SerializeField] private GameObject SelectionMark;

        private IBuildingManager buildingManager;
        
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
        }
        
        protected override void OnSpawn(Unit unit)
        {
            base.OnSpawn(unit);
            unitsToSpawnNumber--;
        }

        protected override bool CanSpawn()
        {
            return unitsToSpawnNumber > 0;
        }

        public void UpgradeBuilding()
        {
            Debug.Log("UpgradeBuilding");
        }

        public virtual void BuildUnit()
        {
            unitsToSpawnNumber++;
        }

        public float GetBuildingProgress()
        {
            if (unitsToSpawnNumber <= 0)
            {
                return 0f;
            }

            float progress = 1f - nextSpawnTimer / nextUnit.SpawnDuration;
            return Mathf.Clamp01(progress);
        }
    }
}
