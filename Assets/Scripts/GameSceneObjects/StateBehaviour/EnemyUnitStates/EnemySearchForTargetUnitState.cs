using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemySearchForTargetUnitState : SearchForTargetUnitState
    {
        private IHeroManager heroManager;
        private EnemyUnit controlledEnemy;
        
        public EnemySearchForTargetUnitState(EnemyUnit unit, EnemyStationBehaviour stateSwitcher, IHeroManager heroManager)
            : base(unit, stateSwitcher)
        {
            this.heroManager = heroManager;
            controlledEnemy = unit;
        }
        
        protected override void ProcessLookForTarget()
        {
            if (heroManager.GetHero().CanBeAttacked() == false)
            {
                return;
            }

            if (HeroIsFar())
            {
                return;
            }
            //TODO: check the Tower
            //TODO: check the hero's allies
            
            controlledEnemy.SetTarget(heroManager.GetHero());
            stateSwitcher.SwitchState<EnemyMoveToTargetUnitState>();
        }
        
        private bool HeroIsFar() //TODO: temporary measure
        {
            float distanceToHero = Vector3.Distance(controlledEnemy.transform.position, heroManager.GetHero().transform.position);
            if (distanceToHero > 5f)
            {
                return true;
            }
            return false;
        }
    }
}
