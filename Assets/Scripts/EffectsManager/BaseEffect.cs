using UnityEngine;

namespace EffectsManager
{
    public class BaseEffect : MonoBehaviour
    {
        [SerializeField] private float duartion = 1f;
        [SerializeField] private ParticleSystem effectPS;
     
        private IEffectHolder effectHolder;
        
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

        public void Emit(IEffectHolder effectHolder, Vector3 from, Vector3 to)
        {
            effectEndTime = Time.time + duartion;
            transform.position = from;
            transform.LookAt(to);
            
            ParticleSystem.MainModule mainModule = effectPS.main;
            float distanceToTarget = Vector3.Distance(to, from);
            float requiredSpeed = distanceToTarget / duartion;
            
            mainModule.startSpeed = requiredSpeed;
            mainModule.startLifetime = duartion;
            
            effectPS.Play();
            PlayOnScene(effectHolder);
        }
    }
}
