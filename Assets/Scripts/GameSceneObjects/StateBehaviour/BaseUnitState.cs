using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public abstract class BaseUnitState
    {
        protected readonly GameUnit GameUnitToControl;
        protected readonly IStateSwitcher stateSwitcher;

        protected BaseUnitState(GameUnit gameUnit, IStateSwitcher stateSwitcher)
        {
            this.stateSwitcher = stateSwitcher;
            GameUnitToControl = gameUnit;
        }
        
        public abstract void OnStateEnter();
        public abstract void OnStateExit();
        public abstract void OnStateUpdate(float deltaTime);
    }
}
