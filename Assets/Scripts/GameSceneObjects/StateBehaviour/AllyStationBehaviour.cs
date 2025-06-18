using EffectsManager;
using GameSceneObjects.Buildings;
using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class AllyStationBehaviour : StationBehaviour
    {
        public AllyStationBehaviour(AllyGameUnit allyGameUnit, IUnitHolder unitHolder,
            IBuildingManager buildingManager, IEffectHolder effectHolder)
        {
            currentUnitState = new AllyIdleState(allyGameUnit, this, buildingManager, unitHolder);
            states.Add(currentUnitState);
            states.Add(new AllyMoveToTargetUnitState(allyGameUnit, this));
            states.Add(new AllyAttackTargetUnitState(allyGameUnit, effectHolder, this));
            states.Add(new AllyMoveToPointUnitState(allyGameUnit, this));
        }
    }
}
