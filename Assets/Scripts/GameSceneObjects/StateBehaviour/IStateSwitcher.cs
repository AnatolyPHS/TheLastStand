namespace GameSceneObjects.StateBehaviour
{
    public interface IStateSwitcher
    {
        void SwitchState<T>() where T : BaseUnitState;
    }
}