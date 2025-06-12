using GameSceneObjects.Units;
using UnityEngine;

namespace Selector
{
    public interface IClickSelectable
    {
        void OnSelect();
        void OnDeselect();
    }
}