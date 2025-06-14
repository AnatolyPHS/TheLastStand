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
        [SerializeField] private float damage = 50f;
        
        public AbilityType AbilityType => abilityType;
        public float CooldownTime => cooldownTime;
        public float Damage => damage;
        public InputType AbilityInputType => abilityInputtype;
    }
}
