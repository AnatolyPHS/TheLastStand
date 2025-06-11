using GameSceneObjects.Buildings;
using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class AllyStationBehaviour : StationBehaviour
    {
        public AllyStationBehaviour(AllyUnit allyUnit, IUnitHolder unitHolder, IBuildingManager buildingManager)
        {
            currentUnitState = new AllyIdleState(allyUnit, this, buildingManager, unitHolder);
            states.Add(currentUnitState);
            states.Add(new AllyMoveToTargetUnitState(allyUnit, this));
            states.Add(new AllyAttackTargetUnitState(allyUnit, this));
            states.Add(new AllyMoveToPointUnitState(allyUnit, this));
        }
    }
}
