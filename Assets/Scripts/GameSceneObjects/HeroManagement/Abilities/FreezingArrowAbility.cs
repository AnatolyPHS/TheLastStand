using EffectsManager;
using GameSceneObjects.Data.AbilityData;
using GameSceneObjects.HeroManagement.Abilities;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class FreezingArrowAbility : AbilityBase
    {
        private IEffectHolder effectHolder;
        private IHeroManager heroManager; 
        
        public override void OnUpdate(Vector3 pointer)
        {
        }

        public override void ActivateAbility(Vector3 mouseGroundPosition)
        {
            abilityController.SetCurrentAbilityType(AbilityType.None);
            effectHolder.ShootEffect(EffectType.FreezeArrow, heroManager.GetHeroPosition(), mouseGroundPosition);
        }

        public FreezingArrowAbility(AbilityBaseInfo abilityBaseInfo, 
            IEffectHolder effectHolder, IHeroManager heroManager, AbilityController controller) 
            : base(abilityBaseInfo, controller)
        {
            this.effectHolder = effectHolder;
            this.heroManager = heroManager;
        }
    }
}
