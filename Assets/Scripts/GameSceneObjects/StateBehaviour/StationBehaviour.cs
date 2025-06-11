using System.Collections.Generic;
using GameSceneObjects.StateBehaviour;

public class StationBehaviour : IStateSwitcher
{
    protected BaseUnitState currentUnitState;
    protected readonly List<BaseUnitState> states = new List<BaseUnitState>();
    
    public void SwitchState<T>() where T : BaseUnitState
    {
        currentUnitState.OnStateExit();
        BaseUnitState newUnitState = states.Find(state => state.GetType() == typeof(T));
        currentUnitState = newUnitState;
        currentUnitState.OnStateEnter();
    }
    
    public void OnUpdate(float deltaTime)
    {
        currentUnitState.OnStateUpdate(deltaTime);
    }
}
