using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class AllyMoveToTargetUnitState : MoveToTargetUnitState
    {
        public AllyMoveToTargetUnitState(Unit unit, StationBehaviour enemyStationBehaviour) : base(unit, enemyStationBehaviour)
        {
        }

        protected override void SwitchToNoTargetState()
        {
            stateSwitcher.SwitchState<AllyIdleState>();
        }
    }
}
