using UnityEngine;

namespace EffectsManager
{
    public class HighlightArea : MonoBehaviour
    {
        [SerializeField] private ParticleSystem highlightParticleSystem;
    
        public void SetRadius(float radius)
        {
            transform.localScale = new Vector3(radius, 1f, radius);
        }
    }
}
