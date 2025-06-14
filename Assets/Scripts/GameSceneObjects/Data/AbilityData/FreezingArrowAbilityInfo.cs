using UnityEngine;

namespace GameSceneObjects.Data.AbilityData
{
    [CreateAssetMenu(fileName = "FreezingArrowAbilityInfo", menuName = "GameData/FreezingArrowAbilityInfo", order = 1)]
    public class FreezingArrowAbilityInfo : AbilityBaseInfo
    {
        [SerializeField] private float freezePoswer = 0.5f;
        [SerializeField] private float freezeDuration = 2f;
        
        public float FreezePower => freezePoswer;
        public float FreezeDuration => freezeDuration;
    }
}
