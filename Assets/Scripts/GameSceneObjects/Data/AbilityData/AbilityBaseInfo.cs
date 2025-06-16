using GameSceneObjects.HeroManagement;
using InputsManager;
using UnityEngine;

namespace GameSceneObjects.Data.AbilityData
{
    public class AbilityBaseInfo : ScriptableObject
    {
        [SerializeField] private AbilityType abilityType = AbilityType.None;
        [SerializeField] private InputType abilityInputtype = InputType.None;
        [SerializeField] private float cooldownTime = 5f;
        [SerializeField] private AnimationCurve damagePerLvl;
        
        
        public AbilityType AbilityType => abilityType;
        public float CooldownTime => cooldownTime;
        public InputType AbilityInputType => abilityInputtype;
        
        public float CalculateDamage(int abiliyLevel)
        {
            return damagePerLvl.Evaluate(abiliyLevel);
        }
    }
}
