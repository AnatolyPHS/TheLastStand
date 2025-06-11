using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemyMoveToTargetUnitState : MoveToTargetUnitState
    {
        public EnemyMoveToTargetUnitState(Unit unit, StationBehaviour enemyStationBehaviour) : base(unit, enemyStationBehaviour)
        {
        }
        
        protected override void SwitchToNoTargetState()
        {
            stateSwitcher.SwitchState<EnemySearchForTargetUnitState>();
        }
    }
}
