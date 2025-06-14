using UnityEngine;

namespace EffectsManager
{
    public enum CursorType
    {
        None = 0,
        MeteorCursor = 10,
        FreezeArrowCursor = 20,
    }
    
    public enum EffectType
    {
        None = 0,
        MeteorShower = 10,
    }
    
    public interface IEffectHolder
    {
        void RemoveFromScene(BaseEffect baseEffect);
        void PlayEffect(EffectType meteorShower, Vector3 mouseGroundPosition, Quaternion identity);
    }
}
