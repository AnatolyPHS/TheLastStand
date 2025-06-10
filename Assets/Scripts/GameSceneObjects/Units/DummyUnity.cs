using Selector;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class DummyUnity : Unit, IClickSelectable
    {
        public void OnSelect()
        {
            Debug.Log("DummyUnity selected! " + gameObject.name);
        }

        public void OnDeselect()
        {
            Debug.Log("DummyUnity deselected! " + gameObject.name);
        }

        public void InteractWithUnit(Unit unt)
        {
            Debug.Log("DummyUnity interacting with unit: " + unt.gameObject.name);
        }

        public void MoveTo(Vector3 targetPosition)
        {
            Debug.Log("DummyUnity moving to position: " + targetPosition + " from position: " + transform.position);
        }
    }
}
