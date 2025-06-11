using Services;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class BuildingManager : MonoBehaviour, IBuildingManager
    {
        [SerializeField] GameObject mainTower;
        [SerializeField] GameObject sanctum;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IBuildingManager>(this);
        }

        public Vector3 GetMainTowerPosition()
        {
            return mainTower.transform.position;
        }

        public Vector3 GetSanctumPosition()
        {
            return sanctum.transform.position;
        }
    }
}
