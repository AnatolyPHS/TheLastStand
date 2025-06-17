using GameSceneObjects.StateBehaviour;
using GameSceneObjects.Units;

public class HeroCalmUnitState : BaseUnitState
{
    private Hero heroToControl;
    
    public HeroCalmUnitState(GameUnit unit, IStateSwitcher stateSwitcher) : base(unit, stateSwitcher)
    {
        heroToControl = unit as Hero;
    }

    public override void OnStateEnter()
    {
        unitToControl.ChangeAnimatorState(GameUnit.Idle01AnimParameter, true);
    }

    public override void OnStateExit()
    {
        unitToControl.ChangeAnimatorState(GameUnit.Idle01AnimParameter, false);
    }

    public override void OnStateUpdate(float deltaTime)
    {
    }
}
