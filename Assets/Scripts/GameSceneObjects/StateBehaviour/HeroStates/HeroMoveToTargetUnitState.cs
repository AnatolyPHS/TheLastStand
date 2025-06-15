using GameSceneObjects.StateBehaviour.HeroStates;
using GameSceneObjects.Units;

public class HeroMoveToTargetUnitState : MoveToTargetUnitState
{
    public HeroMoveToTargetUnitState(Unit unit, StationBehaviour enemyStationBehaviour) : base(unit, enemyStationBehaviour)
    {
    }

    protected override void SwitchToNoTargetState()
    {
        stateSwitcher.SwitchState<HeroCalmUnitState>();
    }

    protected override void SwitchToAttackState()
    {
        stateSwitcher.SwitchState<HeroAttackTargetUnitState>();
    }
}
