using GameSceneObjects.StateBehaviour;
using GameSceneObjects.Units;

public class HeroCalmUnitState : BaseUnitState
{
    private Hero heroToControl;
    
    //TODO: controll hero animations here
    public HeroCalmUnitState(GameUnit gameUnit, IStateSwitcher stateSwitcher) : base(gameUnit, stateSwitcher)
    {
        heroToControl = gameUnit as Hero;
    }

    public override void OnStateEnter()
    {
    }

    public override void OnStateExit()
    {
    }

    public override void OnStateUpdate(float deltaTime)
    {
    }
}
