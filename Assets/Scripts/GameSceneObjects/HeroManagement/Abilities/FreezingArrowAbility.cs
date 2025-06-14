using GameSceneObjects.Data.AbilityData;
using GameSceneObjects.HeroManagement.Abilities;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class FreezingArrowAbility : AbilityBase
    {
        
        public override void OnUpdate(Vector3 pointer)
        {
        }

        public override void ActivateAbility(Vector3 mouseGroundPosition)
        {
            abilityController.SetCurrentAbilityType(AbilityType.None);
            GameObject arrow = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            arrow.transform.position = mouseGroundPosition;
            arrow.transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
            arrow.GetComponent<Renderer>().material.color = Color.blue;
        }

        public FreezingArrowAbility(AbilityBaseInfo abilityBaseInfo, AbilityController controller) : base(abilityBaseInfo, controller)
        {
        }
    }
}
