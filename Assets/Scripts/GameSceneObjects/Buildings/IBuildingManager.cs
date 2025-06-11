using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public interface IBuildingManager 
    {
        Vector3 GetMainTowerPosition();
        Vector3 GetSanctumPosition();
    }
}
