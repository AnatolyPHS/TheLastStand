using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemySearchForTargetUnitState : BaseUnitState
    {
        private const float TickPeriod = 1f;

        private IHeroManager heroManager;
        private EnemyUnit controlledEnemy;
        
        private float nextTickTime = float.MinValue;

        public EnemySearchForTargetUnitState(EnemyUnit unit, EnemyStationBehaviour stateSwitcher, IHeroManager heroManager)
            : base(unit, stateSwitcher)
        {
            this.heroManager = heroManager;
            controlledEnemy = unit;
        }
        
        public override void OnStateEnter()
        {
            ProcessLookForTarget();
            nextTickTime = Time.time + TickPeriod;
        }
        
        public override void OnStateExit()
        {
            nextTickTime = float.MaxValue;
        }

        public override void OnStateUpdate(float deltaTime)
        {
            if (Time.time < nextTickTime)
            {
                return;
            }
            
            nextTickTime = Time.time + TickPeriod;
            ProcessLookForTarget();
        }
        
        private void ProcessLookForTarget()
        {
            if (heroManager.GetHero().IsAlive() == false)
            {
                return;
            }
            
            controlledEnemy.SetTarget(heroManager.GetHero());
            stateSwitcher.SwitchState<MoveToTargetUnitState>();
        }
    }
}
