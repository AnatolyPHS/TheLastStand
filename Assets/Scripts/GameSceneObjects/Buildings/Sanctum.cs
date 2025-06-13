using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;
using Services;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class Sanctum : AllySpawningBuilding
    {
        private IHeroManager heroManager;
        
        public override void BuildUnit()
        {
            unitsToSpawnNumber = 1;
        }
        
        protected override bool CanSpawn()
        {
            return heroManager.HeroIsRespawning() && unitsToSpawnNumber > 0;
        }

        protected override void Start()
        {
            base.Start();
            heroManager = ServiceLocator.Instance.Get<IHeroManager>();
        }
        
        protected override void OnSpawn(Unit unit)
        {
            heroManager.OnHeroRespawn(unit);
            unitsToSpawnNumber = 0;
        }

        public void InstantHeroSpawn(Vector3 position, Quaternion rotation)
        {
            Unit unit = poolManager.GetObject(nextUnit.Unit, position, rotation);
            unit.Init();
            OnSpawn(unit);
        }
    }
}
