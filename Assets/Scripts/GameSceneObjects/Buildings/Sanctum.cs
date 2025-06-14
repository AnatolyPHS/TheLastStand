using System.Collections.Generic;
using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;
using Services;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class Sanctum : AllySpawningBuilding
    {
        private const float TickPeriod = 1f;
        
        [SerializeField] private float healEffect = 10f;
        [SerializeField] private SanctumTrigger sanctumTrigger;
        
        private IHeroManager heroManager;

        private float nextTickTime = float.MinValue;
        
        public override void BuildUnit()
        {
            unitsToSpawnNumber = 1;
        }
        
        public void InstantHeroSpawn(Vector3 position, Quaternion rotation)
        {
            Unit unit = poolManager.GetObject(nextUnit.UnitToSpawn, position, rotation);
            unit.Init();
            OnSpawn(unit);
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

        protected override void Update()
        {
            base.Update();

            ProcessHeal();
        }

        private void ProcessHeal()
        {
            if (Time.time < nextTickTime)
            {
                return;
            }
            
            nextTickTime = Time.time + TickPeriod;

            IReadOnlyList<ISanctumable> UnitsInSanctum = sanctumTrigger.UnitsInSanctum;
            foreach (ISanctumable unit in UnitsInSanctum)
            {
                unit.Heal(healEffect);
            }
        }
    }
}
