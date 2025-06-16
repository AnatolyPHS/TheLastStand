using System.Collections.Generic;
using Camera;
using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;
using Services;
using UI.GameView;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class Sanctum : AllySpawningBuilding
    {
        private const float TickPeriod = 1f;
        
        [SerializeField] private float healEffect = .1f;
        [SerializeField] private SanctumTrigger sanctumTrigger;
        [SerializeField] private AnimationCurve spawnDurationPerLvl;
        
        private IHeroManager heroManager;
        private ICameraController cameraController;

        private float nextTickTime = float.MinValue;
        
        public override void BuildUnit()
        {
            if (heroManager.GetHero().IsAlive())
            {
                return;
            }
            
            if (unitsToSpawnNumber <= 0)
            {
                nextSpawnTimer = CalculateSpawnDuration();
            }
            
            unitsToSpawnNumber = 1;
        }
        
        public void InstantHeroSpawn()
        {
            GameUnit gameUnit = poolManager.GetObject(nextUnit.gameUnitToSpawn, spawnPoint.position, spawnPoint.rotation);
            gameUnit.Init();
            OnSpawn(gameUnit);
        }
        
        protected override bool CanSpawn()
        {
            return heroManager.HeroIsRespawning() && unitsToSpawnNumber > 0;
        }

        protected override void Start()
        {
            base.Start();
            heroManager = ServiceLocator.Instance.Get<IHeroManager>();
            cameraController = ServiceLocator.Instance.Get<ICameraController>();
        }
        
        protected override void OnSpawn(GameUnit gameUnit)
        {
            heroManager.OnHeroRespawn(gameUnit);
            unitsToSpawnNumber = 0;
            cameraController.FocusOnHero();
        }

        protected override void Update()
        {
            base.Update();

            ProcessHeal();
        }

        protected override float CalculateSpawnDuration()
        {
            return base.CalculateSpawnDuration() * spawnDurationPerLvl.Evaluate( heroManager.GetHeroLevel());
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
                unit.Heal(healEffect * currentBuildingLevel);
            }
        }
    }
}
