using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemyAttackTargetUnitState : AttackTargetUnitState
    {
        public EnemyAttackTargetUnitState(Unit unit, StationBehaviour stationBehaviour) : base(unit, stationBehaviour)
        {
        }
        
        protected override void SwitchToMoveToTargetState()
        {
            stateSwitcher.SwitchState<EnemyMoveToTargetUnitState>();
        }

        protected override void SwitchToIdleSate()
        {
            stateSwitcher.SwitchState<EnemySearchForTargetUnitState>();
        }
    }
}
