using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemyMoveToTargetUnitState : MoveToTargetUnitState
    {
        public EnemyMoveToTargetUnitState(GameUnit gameUnit, StationBehaviour enemyStationBehaviour) : base(gameUnit, enemyStationBehaviour)
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
