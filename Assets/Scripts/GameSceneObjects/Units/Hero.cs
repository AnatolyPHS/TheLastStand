using Selector;
using UnityEngine;
using UnityEngine.AI;

namespace GameSceneObjects.Units
{
    public class Hero : Unit, IClickSelectable
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        
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
