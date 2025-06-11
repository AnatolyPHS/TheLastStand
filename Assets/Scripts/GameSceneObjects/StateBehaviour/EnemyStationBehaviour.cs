using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemyStationBehaviour : StationBehaviour
    {
        private readonly EnemyUnit enemyUnit;
        private readonly IHeroManager heroManager;
        
        public EnemyStationBehaviour(EnemyUnit unit, IHeroManager heroManager)
        {
            this.heroManager = heroManager;
            this.enemyUnit = unit;
            
            currentUnitState = new EnemySearchForTargetUnitState(unit, this,  heroManager);
            states.Add(currentUnitState);
            states.Add(new MoveToTargetUnitState(unit, this));
            states.Add(new AttackTargetUnitState(unit, this));
        }
    }
}
