using UnityEngine;

namespace GameSceneObjects.Data.AbilityData
{
    [CreateAssetMenu(fileName = "MeteorShowerAbilityInfo", menuName = "GameData/MeteorShowerAbilityInfo", order = 1)]
    public class MeteorShowerAbilityInfo : AbilityBaseInfo
    {
        [SerializeField] private float radius = 1f;
        
        public float Radius => radius;
    }
}
