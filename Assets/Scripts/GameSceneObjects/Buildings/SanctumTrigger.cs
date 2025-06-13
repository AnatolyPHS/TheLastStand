using System.Collections.Generic;
using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class SanctumTrigger : MonoBehaviour
    {
        private readonly List<ISanctumable> unitsInSanctum = new List<ISanctumable>();
        
        public IReadOnlyList<ISanctumable> UnitsInSanctum => unitsInSanctum;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ISanctumable>(out ISanctumable unit) == false)
            {
                return;
            }
            
            unitsInSanctum.Add(unit);
            unit.OnSanctumeEnter();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<ISanctumable>(out ISanctumable unit) == false)
            {
                return;
            }
            
            if (unitsInSanctum.Contains(unit))
            {
                unit.OnSanctumExit();
                unitsInSanctum.Remove(unit);
            }
        }
    }
}
