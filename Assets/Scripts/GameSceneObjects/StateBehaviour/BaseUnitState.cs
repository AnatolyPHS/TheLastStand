using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public abstract class BaseUnitState
    {
        protected readonly GameUnit UnitToControl;
        protected readonly IStateSwitcher stateSwitcher;

        protected BaseUnitState(GameUnit unit, IStateSwitcher stateSwitcher)
        {
            this.stateSwitcher = stateSwitcher;
            UnitToControl = unit;
        }
        
        public abstract void OnStateEnter();
        public abstract void OnStateExit();
        public abstract void OnStateUpdate(float deltaTime);
    }
}
