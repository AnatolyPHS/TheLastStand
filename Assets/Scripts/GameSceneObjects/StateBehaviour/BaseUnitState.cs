using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public abstract class BaseUnitState
    {
        protected readonly Unit unitToControl;
        protected readonly IStateSwitcher stateSwitcher;

        protected BaseUnitState(Unit unit, IStateSwitcher stateSwitcher)
        {
            this.stateSwitcher = stateSwitcher;
            unitToControl = unit;
        }
        
        public abstract void OnStateEnter();
        public abstract void OnStateExit();
        public abstract void OnStateUpdate(float deltaTime);
    }
}
