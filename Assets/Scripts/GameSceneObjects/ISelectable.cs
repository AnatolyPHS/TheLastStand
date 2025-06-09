using UnityEngine;

namespace Selector
{
    internal interface ISelectable
    {
        void OnSelect();
        void OnDeselect();
    }
}