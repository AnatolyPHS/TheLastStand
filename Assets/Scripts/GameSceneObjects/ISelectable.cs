using UnityEngine;

namespace Selector
{
    internal interface ISelectable
    {
        Vector3 GetVisualPosition();
        void OnSelect();
        void OnDeselect();
    }
}