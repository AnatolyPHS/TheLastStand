using GameSceneObjects.Units;
using UnityEngine;

namespace Selector
{
    internal interface IClickSelectable
    {
        void OnSelect();
        void OnDeselect();
        void InteractWithUnit(Unit unt);
        void MoveTo(Vector3 targetPosition);
    }
}