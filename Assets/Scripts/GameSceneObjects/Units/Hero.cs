using Selector;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class Hero : Unit, IClickSelectable
    {
        public void OnSelect()
        {
            Debug.Log("Hero selected! " + gameObject.name);
        }

        public void OnDeselect()
        {
            Debug.Log("Hero deselected! " + gameObject.name);
        }

        public void InteractWithUnit(Unit unt)
        {
            Debug.Log("Hero interacting with unit: " + unt.gameObject.name);
        }

        public void MoveTo(Vector3 targetPosition)
        {
            navMeshAgent.SetDestination(targetPosition);
        }
    }
}
