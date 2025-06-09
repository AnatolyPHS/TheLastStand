using Selector;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class DummyUnity : Unit, ISelectable
    {
        public void OnSelect()
        {
            Debug.Log("DummyUnity selected! " + gameObject.name);
        }

        public void OnDeselect()
        {
            Debug.Log("DummyUnity deselected! " + gameObject.name);
        }
    }
}
