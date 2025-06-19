using GameSceneObjects.Buildings;

internal interface IBuildingViewController 
{
    void SetSelectedAllyBuilding(AllySpawningBuilding building);
    AllySpawningBuilding GetSelectedAllyBuilding();
    void OnUpgradeClick(float f);
}
