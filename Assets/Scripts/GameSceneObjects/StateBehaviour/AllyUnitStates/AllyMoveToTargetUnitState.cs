using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class AllyMoveToTargetUnitState : MoveToTargetUnitState
    {
        public AllyMoveToTargetUnitState(GameUnit unit, StationBehaviour enemyStationBehaviour) : base(unit, enemyStationBehaviour)
        {
        }

        protected override void SwitchToNoTargetState()
        {
            stateSwitcher.SwitchState<AllyIdleState>();
        }
        
        protected override void SwitchToAttackState()
        {
            stateSwitcher.SwitchState<AllyAttackTargetUnitState>();
        }
    }
}
