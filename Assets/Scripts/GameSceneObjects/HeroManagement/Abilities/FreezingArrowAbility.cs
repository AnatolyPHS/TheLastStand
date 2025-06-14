using GameSceneObjects.Data.AbilityData;
using GameSceneObjects.HeroManagement.Abilities;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class FreezingArrowAbility : AbilityBase
    {
        
        public override void OnUpdate(Vector3 pointer)
        {
            throw new System.NotImplementedException();
        }

        public override void ActivateAbility(Vector3 mouseGroundPosition)
        {
            throw new System.NotImplementedException();
        }

        public FreezingArrowAbility(AbilityBaseInfo abilityBaseInfo, AbilityController controller) : base(abilityBaseInfo, controller)
        {
        }
    }
}
