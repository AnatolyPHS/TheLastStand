using UnityEngine;

namespace EffectsManager
{
    public enum CursorType
    {
        Default = 0,
        Magic = 20,
    }
    
    public enum HighlightAreaType
    {
        None = 0,
        MeteorShowerArea = 10,
    }
    
    public enum EffectType
    {
        None = 0,
        MeteorShower = 10,
        FreezeArrow = 20,
    }
    
    public interface IEffectHolder
    {
        void RemoveFromScene(BaseEffect baseEffect);
        void PlayEffect(EffectType meteorShower, Vector3 mouseGroundPosition, Quaternion identity);
        void ShootEffect(EffectType freezeArrow, Vector3 from, Vector3 to);
        HighlightArea GetHighlightAreaEffect();
        void RemoveHighlightAreaEffect(HighlightArea selectionArea);
        void ChangeCursor(CursorType freezeArrowCursor);
    }
}
