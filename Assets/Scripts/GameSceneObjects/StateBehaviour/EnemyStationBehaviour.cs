using EffectsManager;
using GameSceneObjects.Buildings;
using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemyStationBehaviour : StationBehaviour
    { 
        public EnemyStationBehaviour(EnemyGameUnit gameUnit, IHeroManager heroManager,
            IBuildingManager buildingManager, IUnitHolder unitHolder, IEffectHolder effectHolder)
        {
            currentUnitState = new EnemySearchForTargetUnitState(gameUnit, this, heroManager, buildingManager, unitHolder);
            states.Add(currentUnitState);
            states.Add(new EnemyMoveToTargetUnitState(gameUnit, heroManager, unitHolder, this));
            states.Add(new EnemyAttackTargetUnitState(gameUnit, effectHolder, this));
        }
    }
}
