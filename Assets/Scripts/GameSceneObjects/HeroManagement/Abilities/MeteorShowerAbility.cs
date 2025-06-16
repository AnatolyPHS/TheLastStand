using EffectsManager;
using GameSceneObjects.Data.AbilityData;
using GameSceneObjects.HeroManagement.Abilities;
using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class MeteorShowerAbility : AbilityBase
    {
        private IEffectHolder effectHolder;
        private HighlightArea selectionArea;

        private MeteorShowerAbilityInfo meteorShowerAbilityInfo;
        
        public override void AbilityButtonClick(float f)
        {
            base.AbilityButtonClick(f);

            selectionArea = effectHolder.GetHighlightAreaEffect();
            selectionArea.SetRadius(meteorShowerAbilityInfo.Radius);
            
            effectHolder.ChangeCursor(CursorType.Magic);
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
            
            DamageEnemiesInArea(mouseGroundPosition);
            
            effectHolder.ChangeCursor(CursorType.Default);
        }

        private void DamageEnemiesInArea(Vector3 mouseGroundPosition)
        {
            Collider[] hitColliders = Physics.OverlapSphere(mouseGroundPosition, meteorShowerAbilityInfo.Radius);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out EnemyUnit enemy))
                {
                    enemy.GetDamage(meteorShowerAbilityInfo.CalculateDamage(abiliyLevel));//TODO: Add damage scaling via animation curve
                }
            }
        }

        public MeteorShowerAbility(AbilityBaseInfo abilityBaseInfo, IEffectHolder effectHolder,
            AbilityController controller) : base(abilityBaseInfo, controller)
        {
            this.effectHolder = effectHolder; 
            meteorShowerAbilityInfo = abilityBaseInfo as MeteorShowerAbilityInfo;
        }
    }
}
