using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemyStationBehaviour : StationBehaviour
    { 
        public EnemyStationBehaviour(EnemyUnit unit, IHeroManager heroManager)
        {
            currentUnitState = new EnemySearchForTargetUnitState(unit, this,  heroManager);
            states.Add(currentUnitState);
            states.Add(new EnemyMoveToTargetUnitState(unit, this));
            states.Add(new EnemyAttackTargetUnitState(unit, this));
        }
    }
}
