using GameSceneObjects.Buildings;
using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemySearchForTargetUnitState : SearchForTargetUnitState
    {
        private IHeroManager heroManager;
        private IBuildingManager buildingManager;
        private IUnitHolder unitHolder;
        
        private EnemyGameUnit controlledEnemy;
        
        public EnemySearchForTargetUnitState(EnemyGameUnit unit, EnemyStationBehaviour stateSwitcher, IHeroManager heroManager,
            IBuildingManager buildingManager, IUnitHolder unitHolder)
            : base(unit, stateSwitcher)
        {
            this.heroManager = heroManager;
            controlledEnemy = unit;
            this.buildingManager = buildingManager;
            this.unitHolder = unitHolder;
        }
        
        protected override void ProcessLookForTarget()
        {
            IHittable target = GetNearestTarget();
            
            controlledEnemy.SetTarget(target);
            stateSwitcher.SwitchState<EnemyMoveToTargetUnitState>();
        }

        private IHittable GetNearestTarget()
        {
            Hero hero = heroManager.GetHero();
            if (hero.CanBeAttacked() &&
                Vector3.Distance(hero.transform.position, controlledEnemy.transform.position) <= controlledEnemy.GetSearchRadius())
            {
                return hero;
            }
            
            if (unitHolder.TryGetGlosestUnit(UnitFaction.Ally, controlledEnemy.transform.position, out GameUnit closestEnemy))
            {
                return closestEnemy;
            }

            return buildingManager.GetCitadel();
        }
    }
}
