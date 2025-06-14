using UnityEngine;

namespace Selector
{
    public interface ISelectorController
    {
        void DeselectObject(IClickSelectable selectable);
        Vector3 RecalculateWorldPointUnderMouse(Vector2 screenPosition);
    }
}
