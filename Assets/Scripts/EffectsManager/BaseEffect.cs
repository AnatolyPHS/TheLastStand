using UnityEngine;

namespace EffectsManager
{
    public class BaseEffect : MonoBehaviour
    {
        private IEffectHolder effectHolder;
        
        [SerializeField] private float duartion = 1f;
        
        private float effectEndTime;

        public void PlayOnScene(IEffectHolder effectHolder)
        {
            this.effectHolder = effectHolder;
            effectEndTime = Time.time + duartion;
        }
        
        private void Update()
        {
            if (Time.time < effectEndTime)
            {
                return;
            }
            
            effectHolder.RemoveFromScene(this);
        }
    }
}
