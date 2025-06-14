using GameSceneObjects.Units;
using UnityEngine;

namespace Selector
{
    public interface ISelectorController
    {
        void DeselectObject(IClickSelectable selectable);
        Vector3 GetCurrentWorldPoint();
    }
}
