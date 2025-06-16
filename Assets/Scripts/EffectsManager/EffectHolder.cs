using System;
using System.Collections.Generic;
using PoolingSystem;
using Services;
using UnityEngine;

namespace EffectsManager
{
    public class EffectHolder : MonoBehaviour, IEffectHolder
    {
        [SerializeField] private List<EffectEntry> effects = new List<EffectEntry>();
        
        private IPoolManager poolManager;
        
        private Dictionary<EffectType, BaseEffect> effectPrefabs = new Dictionary<EffectType, BaseEffect>();
        
        public void RemoveFromScene(BaseEffect baseEffect)
        {
            poolManager.ReturnObject(baseEffect);
        }

        public void PlayEffect(EffectType meteorShower, Vector3 mouseGroundPosition, Quaternion identity)
        {
            if (!effectPrefabs.TryGetValue(meteorShower, out BaseEffect effectPrefab))
            {
                Debug.LogError($"Effect of type {meteorShower} not found.");
                return;
            }

            BaseEffect effectInstance = poolManager.GetObject(effectPrefab, mouseGroundPosition, identity);
            
            effectInstance.PlayOnScene(this);
        }

        public void ShootEffect(EffectType freezeArrow, Vector3 from, Vector3 to)
        {
            if (!effectPrefabs.TryGetValue(freezeArrow, out BaseEffect effectPrefab))
            {
                Debug.LogError($"Effect of type {freezeArrow} not found.");
                return;
            }

            BaseEffect effectInstance = poolManager.GetObject(effectPrefab, from, Quaternion.identity);
            
            effectInstance.Emit(this, from, to);
        }

        private void Awake()
        {
            ServiceLocator.Instance.Register<IEffectHolder>(this);
        }

        private void Start()
        {
            poolManager = ServiceLocator.Instance.Get<IPoolManager>();
            foreach (EffectEntry effect in effects)
            {
                if (effectPrefabs.ContainsKey(effect.GetEffectType))
                {
                    continue;
                }
                
                effectPrefabs.Add(effect.GetEffectType, effect.GetEffectPrefab);
            }
        }
        
        private void OnDestroy()
        {
            ServiceLocator.Instance.Unregister<IEffectHolder>();
        }
    }

    [Serializable]
    public class EffectEntry
    {
        [SerializeField] private EffectType effectType;
        [SerializeField] private BaseEffect effectPrefab;
        
        public EffectType GetEffectType => effectType;
        public BaseEffect GetEffectPrefab => effectPrefab;
    }
}
