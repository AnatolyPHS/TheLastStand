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
        EnemyPointer = 30,
        AllyArrow =40,
        EnemyArrow = 50,
    }
    
    public interface IEffectHolder
    {
        void RemoveFromScene(BaseEffect baseEffect);
        void PlayEffect(EffectType effect, Vector3 mouseGroundPosition, Quaternion identity);
        void PlayEffect(EffectType effect, Vector3 mouseGroundPosition, Quaternion identity, Transform parent = null);
        void ShootEffect(EffectType effect, Vector3 from, Vector3 to);
        HighlightArea GetHighlightAreaEffect();
        void RemoveHighlightAreaEffect(HighlightArea selectionArea);
        void ChangeCursor(CursorType freezeArrowCursor);
    }
}
