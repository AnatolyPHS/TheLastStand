using GameSceneObjects.StateBehaviour.HeroStates;
using GameSceneObjects.Units;

public class HeroMoveToTargetUnitState : MoveToTargetUnitState
{
    public HeroMoveToTargetUnitState(GameUnit gameUnit, StationBehaviour enemyStationBehaviour) : base(gameUnit, enemyStationBehaviour)
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
