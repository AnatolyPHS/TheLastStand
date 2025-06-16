using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour.HeroStates
{
    public class HeroAttackTargetUnitState : AttackTargetUnitState
    {
        public HeroAttackTargetUnitState(GameUnit unit, StationBehaviour stationBehaviour) : base(unit, stationBehaviour)
        {
        }

        protected override void SwitchToMoveToTargetState()
        {
            stateSwitcher.SwitchState<HeroCalmUnitState>();
        }

        protected override void SwitchToIdleSate()
        {
            stateSwitcher.SwitchState<HeroCalmUnitState>();
        }
        
    }
}

