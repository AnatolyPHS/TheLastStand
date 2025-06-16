using GameSceneObjects.Units;
using Selector;
using UnityEngine;

namespace GameSceneObjects
{
    public interface IClickInteractable : IClickSelectable
    {
        void InteractWithUnit(GameUnit unt);
        void MoveTo(Vector3 targetPosition);
    }
}
