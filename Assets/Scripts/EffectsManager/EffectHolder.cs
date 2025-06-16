using System;
using System.Collections.Generic;
using PoolingSystem;
using Services;
using UnityEngine;

namespace EffectsManager
{
    public class EffectHolder : MonoBehaviour, IEffectHolder
    {
        public const float TargetPointerShowDelay = 1f;
        
        [SerializeField] private List<EffectEntry> effects = new List<EffectEntry>(); //TODO: need a serializable dictionary
        [SerializeField] private List<HighlightAreaEntry> highlightAreas = new List<HighlightAreaEntry>();
        [SerializeField] public Texture2D abilityCursorTexture;
        
        private IPoolManager poolManager;
        
        private Dictionary<EffectType, BaseEffect> effectPrefabs = new Dictionary<EffectType, BaseEffect>();
        private Dictionary<HighlightAreaType, HighlightArea> highlightAreaPrefabs = new Dictionary<HighlightAreaType, HighlightArea>();
        
        public void RemoveFromScene(BaseEffect baseEffect)
        {
            poolManager.ReturnObject(baseEffect);
        }

        public void PlayEffect(EffectType meteorShower, Vector3 mouseGroundPosition, Quaternion identity)
        {
            TryToPlayEffect(meteorShower, mouseGroundPosition, identity, out _);
        }

        public void PlayEffect(EffectType meteorShower, Vector3 mouseGroundPosition, Quaternion identity, Transform parent = null)
        {
            if (TryToPlayEffect(meteorShower, mouseGroundPosition, identity, out BaseEffect effectInstance) == false)
            {
                return;
            }

            effectInstance.transform.SetParent(parent);
        }

        private bool TryToPlayEffect(EffectType meteorShower, Vector3 mouseGroundPosition, Quaternion identity,
            out BaseEffect effectInstance)
        {
            if (!effectPrefabs.TryGetValue(meteorShower, out BaseEffect effectPrefab))
            {
                Debug.LogError($"Effect of type {meteorShower} not found.");
                effectInstance = null;
                return false;
            }

            effectInstance = poolManager.GetObject(effectPrefab, mouseGroundPosition, identity);
            
            effectInstance.PlayOnScene(this);
            return true;
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

        public HighlightArea GetHighlightAreaEffect()
        {
            if (!highlightAreaPrefabs.TryGetValue(HighlightAreaType.MeteorShowerArea, out HighlightArea highlightAreaPrefab))
            {
                Debug.LogError("Highlight area of type MeteorShowerArea not found.");
                return null;
            }

            HighlightArea highlightAreaInstance = poolManager.GetObject(highlightAreaPrefab, Vector3.zero, Quaternion.identity);
            return highlightAreaInstance;
        }

        public void RemoveHighlightAreaEffect(HighlightArea selectionArea)
        {
            poolManager.ReturnObject(selectionArea);
        }

        public void ChangeCursor(CursorType cursorType)
        {
            Cursor.SetCursor(cursorType == CursorType.Magic ? abilityCursorTexture : null,
                Vector2.zero, CursorMode.Auto);
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
            
            foreach (HighlightAreaEntry highlightArea in highlightAreas)
            {
                if (highlightAreaPrefabs.ContainsKey(highlightArea.GetHighlightAreaType))
                {
                    continue;
                }
                
                highlightAreaPrefabs.Add(highlightArea.GetHighlightAreaType, highlightArea.GetHighlightAreaPrefab);
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

    [Serializable]
    public class HighlightAreaEntry
    {
        [SerializeField] private HighlightAreaType highlightAreaType;
        [SerializeField] private HighlightArea highlightAreaPrefab;

        public HighlightAreaType GetHighlightAreaType => highlightAreaType;
        public HighlightArea GetHighlightAreaPrefab => highlightAreaPrefab;
    }
}
