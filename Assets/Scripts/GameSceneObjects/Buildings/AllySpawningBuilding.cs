using GameSceneObjects.Units;
using Selector;
using UnityEngine;


namespace GameSceneObjects.Buildings
{
    public class AllySpawningBuilding : SpawningBuilding, IClickSelectable
    {
        [SerializeField] private GameObject SelectionMark;

        private int unitsToSpawnNumber = 0;
        
        public void OnSelect()
        {
            SelectionMark.SetActive(true);
            unitsToSpawnNumber++;
        }

        public void OnDeselect()
        {
            SelectionMark.SetActive(false);
        }
        
        protected override void Start()
        {
            base.Start();
            // add UI view reference to controll;
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
    }
}
