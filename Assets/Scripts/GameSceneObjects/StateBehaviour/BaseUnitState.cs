using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public abstract class BaseUnitState
    {
        protected readonly GameUnit unitToControl;
        protected readonly IStateSwitcher stateSwitcher;

        protected BaseUnitState(GameUnit unit, IStateSwitcher stateSwitcher)
        {
            this.stateSwitcher = stateSwitcher;
            unitToControl = unit;
        }
        
        public abstract void OnStateEnter();
        public abstract void OnStateExit();
        public abstract void OnStateUpdate(float deltaTime);
    }
}
