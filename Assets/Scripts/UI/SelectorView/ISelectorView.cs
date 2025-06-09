using UnityEngine;

namespace UI.SelectorView
{
    public interface ISelectorView 
    {
        void SetSelectorState(bool newState);
        void UpdateSelector(Vector3 startWorldPoint, Vector3 currentWorldPoint, float minDragDistance);
    }
}
