using EffectsManager;
using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class AllyAttackTargetUnitState : AttackTargetUnitState
    {
        public AllyAttackTargetUnitState(GameUnit unit, IEffectHolder effectManager, StationBehaviour stationBehaviour)
            : base(unit, effectManager, stationBehaviour)
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
