using EffectsManager;
using GameSceneObjects.Data.AbilityData;
using GameSceneObjects.HeroManagement.Abilities;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class MeteorShowerAbility : AbilityBase
    {
        private IEffectHolder effectHolder;
        private HighlightArea selectionArea;
        
        private float radius = 1;
        
        public override void AbilityButtonClick(float f)
        {
            base.AbilityButtonClick(f);

            selectionArea = effectHolder.GetHighlightAreaEffect();
            selectionArea.SetRadius(radius);
        }
        
        public override void OnUpdate(Vector3 pointer)
        {
            if (abilityInUse)
            {
                selectionArea.transform.position = pointer;
            }
        }

        public override void ActivateAbility(Vector3 mouseGroundPosition)
        {
            abilityController.SetCurrentAbilityType(AbilityType.None);
            effectHolder.PlayEffect(EffectType.MeteorShower, mouseGroundPosition, Quaternion.identity);
            effectHolder.RemoveHighlightAreaEffect(selectionArea);
            abilityInUse = false;
        }

        public MeteorShowerAbility(AbilityBaseInfo abilityBaseInfo, IEffectHolder effectHolder,
            AbilityController controller) : base(abilityBaseInfo, controller)
        {
            this.effectHolder = effectHolder;
            MeteorShowerAbilityInfo meteorShowerAbilityInfo = abilityBaseInfo as MeteorShowerAbilityInfo;
            
            radius = meteorShowerAbilityInfo.Radius;
        }
    }
}
