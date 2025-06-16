using UnityEngine;

namespace GameSceneObjects.Units.Buffs
{
    public class SlowDebuff : BuffBase
    {
        private float initialSpeed;
        private float initialAttackSpeedFactor;
        
        public SlowDebuff()
        {
            buffType = UnitBuffType.Slow;
        }
        
        public override void OnImplement(EnemyUnit target, float duration, float power)
        {
            float effectMultiplier = 1f - power;
            initialSpeed = target.GetNavMeshAgent().speed;
            target.GetNavMeshAgent().speed *= effectMultiplier;
            
            initialAttackSpeedFactor = target.AttackSpeedFactor;
            target.AttackSpeedFactor *= effectMultiplier;
            
            endTime = Time.time + duration;
        }
        
        public override void OnRemoveDebuff(EnemyUnit target)
        {
            target.GetNavMeshAgent().speed = initialSpeed; //TODO: check the allocation!
            target.AttackSpeedFactor = initialAttackSpeedFactor;
        }

        public override void RefreshBuff(float duration, float power)
        {
            endTime = Time.time + duration;
        }
    }
}
