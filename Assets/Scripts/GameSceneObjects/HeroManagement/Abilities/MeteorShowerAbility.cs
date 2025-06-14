using EffectsManager;
using GameSceneObjects.Data.AbilityData;
using GameSceneObjects.HeroManagement.Abilities;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class MeteorShowerAbility : AbilityBase
    {
        private IEffectHolder effectHolder;
        
        public override void OnUpdate(Vector3 pointer)
        {
        }

        public override void ActivateAbility(Vector3 mouseGroundPosition)
        {
            abilityController.SetCurrentAbilityType(AbilityType.None);
            effectHolder.PlayEffect(EffectType.MeteorShower, mouseGroundPosition, Quaternion.identity);
        }

        public MeteorShowerAbility(AbilityBaseInfo abilityBaseInfo, IEffectHolder effectHolder,
            AbilityController controller) : base(abilityBaseInfo, controller)
        {
            this.effectHolder = effectHolder;
        }
    }
}
