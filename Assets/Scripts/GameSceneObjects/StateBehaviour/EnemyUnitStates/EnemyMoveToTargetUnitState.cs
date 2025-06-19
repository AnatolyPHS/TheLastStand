using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemyMoveToTargetUnitState : MoveToTargetUnitState
    {
        private IHeroManager heroManager;
        private IUnitHolder unitHolder;
        
        public EnemyMoveToTargetUnitState(GameUnit unit, IHeroManager heroManager, IUnitHolder unitHolder,
            StationBehaviour enemyStationBehaviour) : base(unit, enemyStationBehaviour)
        {
            this.heroManager = heroManager;
            this.unitHolder = unitHolder;
        }
        
        protected override void ProcessTargetApproach()
        {
            if (unitWithTarget.HasTarget() == false || unitWithTarget.GetCurrentTarget().CanBeAttacked() == false)
            {
                stateSwitcher.SwitchState<EnemySearchForTargetUnitState>();
                return;
            }
            
            IHittable target = unitWithTarget.GetCurrentTarget();
            IHittable closestTarget = GetClosestTarget();
            
            if (closestTarget != target)
            {
                unitWithTarget.SetTarget(closestTarget);
                target = closestTarget;
            }
            
            Vector3 targetPosition = target.GetPosition();
            controllableAgent.SetDestination(targetPosition);

            if (Vector3.Distance(unitToControl.transform.position, targetPosition) <= unitToControl.GetAttackRange())
            {
                stateSwitcher.SwitchState<EnemyAttackTargetUnitState>();
            }
        }

        private IHittable GetClosestTarget()
        {
            Hero hero = heroManager.GetHero();
            if (hero.CanBeAttacked() &&
                Vector3.Distance(hero.transform.position, unitToControl.transform.position) <= unitToControl.GetSearchRadius())
            {
                return hero;
            }
            
            if (unitHolder.TryGetGlosestUnit(UnitFaction.Ally, unitToControl.transform.position, out GameUnit closestTarget) && closestTarget.CanBeAttacked()
                && Vector3.Distance(closestTarget.transform.position, unitToControl.transform.position) <= unitToControl.GetSearchRadius())
            {
                return closestTarget;
            }
            
            return unitWithTarget.GetCurrentTarget();
        }
    }
}
