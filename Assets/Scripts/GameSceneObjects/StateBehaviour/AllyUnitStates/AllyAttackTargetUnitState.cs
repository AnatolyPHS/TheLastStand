using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class AllyAttackTargetUnitState : AttackTargetUnitState
    {
        public AllyAttackTargetUnitState(GameUnit gameUnit, StationBehaviour stationBehaviour) : base(gameUnit, stationBehaviour)
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
