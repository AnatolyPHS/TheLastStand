using GameSceneObjects.Data.AbilityData;
using GameSceneObjects.HeroManagement.Abilities;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class MeteorShowerAbility : AbilityBase
    {
        public override void OnUpdate(Vector3 pointer)
        {
        }

        public override void ActivateAbility(Vector3 mouseGroundPosition)
        {
            GameObject meteor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            meteor.transform.position = mouseGroundPosition;
        }

        public MeteorShowerAbility(AbilityBaseInfo abilityBaseInfo, AbilityController controller) : base(abilityBaseInfo, controller)
        {
        }
    }
}
