
namespace GameSceneObjects.Units.Buffs
{
    public abstract class BuffBase
    {
        protected float endTime;
        
        protected UnitBuffType buffType;
        
        public UnitBuffType GetBuffType => buffType;
        public float EndTime => endTime;
        
        public abstract void OnImplement(EnemyUnit target, float duration, float power);
        public abstract void OnRemoveDebuff(EnemyUnit target);

        public abstract void RefreshBuff(float freezeDuration, float power);
    }
}
