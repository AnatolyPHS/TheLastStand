using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class AllyAttackTargetUnitState : AttackTargetUnitState
    {
        public AllyAttackTargetUnitState(Unit unit, StationBehaviour stationBehaviour) : base(unit, stationBehaviour)
        {
        }
        
        protected override void SwitchToMoveToTargetState()
        {
            stateSwitcher.SwitchState<AllyMoveToTargetUnitState>();
        }

        protected override void SwitchToIdleSate()
        {
            stateSwitcher.SwitchState<AllyIdleState>();
        }
    }
}
