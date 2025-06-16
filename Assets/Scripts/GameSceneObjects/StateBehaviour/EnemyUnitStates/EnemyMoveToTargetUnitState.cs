using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemyMoveToTargetUnitState : MoveToTargetUnitState
    {
        public EnemyMoveToTargetUnitState(GameUnit unit, StationBehaviour enemyStationBehaviour) : base(unit, enemyStationBehaviour)
        {
        }
        
        protected override void SwitchToNoTargetState()
        {
            stateSwitcher.SwitchState<EnemySearchForTargetUnitState>();
        }
        
        protected override void SwitchToAttackState()
        {
            stateSwitcher.SwitchState<EnemyAttackTargetUnitState>();
        }
    }
}
