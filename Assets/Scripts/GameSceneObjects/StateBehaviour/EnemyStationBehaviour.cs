using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemyStationBehaviour : StationBehaviour
    { 
        public EnemyStationBehaviour(EnemyGameUnit gameUnit, IHeroManager heroManager)
        {
            currentUnitState = new EnemySearchForTargetUnitState(gameUnit, this,  heroManager);
            states.Add(currentUnitState);
            states.Add(new EnemyMoveToTargetUnitState(gameUnit, this));
            states.Add(new EnemyAttackTargetUnitState(gameUnit, this));
        }
    }
}
