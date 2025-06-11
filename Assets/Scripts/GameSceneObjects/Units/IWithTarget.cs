namespace GameSceneObjects.Units
{
    public interface IWithTarget
    {
        void SetTarget(IHittable target);
        IHittable GetCurrentTarget();
        bool HasTarget();
        void Attack();
    }
}