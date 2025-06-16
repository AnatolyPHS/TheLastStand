using EffectsManager;
using GameSceneObjects.Data.AbilityData;
using GameSceneObjects.HeroManagement.Abilities;
using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class FreezingArrowAbility : AbilityBase
    {
        private const float EnemyCheckRadius = 1f;
        
        private IEffectHolder effectHolder;
        private IHeroManager heroManager;

        private FreezingArrowAbilityInfo freezingArrowInfo;
        
        public override void AbilityButtonClick(float f)
        {
            base.AbilityButtonClick(f);
            effectHolder.ChangeCursor(CursorType.Magic);
        }
        
        public override void OnUpdate(Vector3 pointer)
        {
        }

        public override void ActivateAbility(Vector3 mouseGroundPosition)
        {
            abilityController.SetCurrentAbilityType(AbilityType.None);
            effectHolder.ShootEffect(EffectType.FreezeArrow, heroManager.GetHeroPosition(), mouseGroundPosition);

            DamageEnemy(mouseGroundPosition);
            
            effectHolder.ChangeCursor(CursorType.Default);
        }

        private void DamageEnemy(Vector3 mouseGroundPosition)
        {
            Collider[] hitColliders = Physics.OverlapSphere(mouseGroundPosition, EnemyCheckRadius);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out EnemyGameUnit enemy))
                {
                    enemy.GetDamage(freezingArrowInfo.CalculateDamage(abiliyLevel));
                    enemy.AddBuff(UnitBuffType.Slow, freezingArrowInfo.FreezePower, freezingArrowInfo.FreezeDuration);
                    return;
                }
            }
        }

        public FreezingArrowAbility(AbilityBaseInfo abilityBaseInfo, 
            IEffectHolder effectHolder, IHeroManager heroManager, AbilityController controller) 
            : base(abilityBaseInfo, controller)
        {
            this.effectHolder = effectHolder;
            this.heroManager = heroManager;
            
            freezingArrowInfo = abilityBaseInfo as FreezingArrowAbilityInfo;
        }
    }
}
