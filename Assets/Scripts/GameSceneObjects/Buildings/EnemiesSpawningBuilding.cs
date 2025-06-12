using GameSceneObjects.Units;

namespace GameSceneObjects.Buildings
{
    public class EnemiesSpawningBuilding : SpawningBuilding
    {
        protected override bool CanSpawn()
        {
            return unitsHolder.GetUnitsCount(UnitFaction.Enemy) < currentBuildingLevel;
        }
    }
}
