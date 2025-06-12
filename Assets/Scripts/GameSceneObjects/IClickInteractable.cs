using GameSceneObjects.Units;
using Selector;
using UnityEngine;

namespace GameSceneObjects
{
    public interface IClickInteractable : IClickSelectable
    {
        void InteractWithUnit(Unit unt);
        void MoveTo(Vector3 targetPosition);
    }
}
