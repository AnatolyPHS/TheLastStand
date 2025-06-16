using GameSceneObjects.HeroManagement;
using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemySearchForTargetUnitState : SearchForTargetUnitState
    {
        private IHeroManager heroManager;
        private EnemyGameUnit _controlledEnemyGame;
        
        public EnemySearchForTargetUnitState(EnemyGameUnit gameUnit, EnemyStationBehaviour stateSwitcher, IHeroManager heroManager)
            : base(gameUnit, stateSwitcher)
        {
            this.heroManager = heroManager;
            _controlledEnemyGame = gameUnit;
        }
        
        protected override void ProcessLookForTarget()
        {
            if (heroManager.GetHero().CanBeAttacked() == false)
            {
                return;
            }

            if (HeroIsFar())
            {
                return;
            }
            //TODO: check the Tower
            //TODO: check the hero's allies
            
            _controlledEnemyGame.SetTarget(heroManager.GetHero());
            stateSwitcher.SwitchState<EnemyMoveToTargetUnitState>();
        }
        
        private bool HeroIsFar() //TODO: temporary measure
        {
            float distanceToHero = Vector3.Distance(_controlledEnemyGame.transform.position, heroManager.GetHero().transform.position);
            if (distanceToHero > 5f)
            {
                return true;
            }
            return false;
        }
    }
}
