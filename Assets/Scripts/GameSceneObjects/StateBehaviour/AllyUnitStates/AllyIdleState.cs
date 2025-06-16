using GameSceneObjects.Buildings;
using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.StateBehaviour
{
    public class AllyIdleState : BaseUnitState
    { 
        private const float TickDuraction = 1.5f;
        private const float HPAllowedThreshold = 0.5f;

        private IBuildingManager buildingManager;
        private IUnitHolder unitHolder;

        private AllyGameUnit _allyGameUnitToControl;
        private float nextTickTime = float.MinValue;

        private EnemyGameUnit _closestEnemyGame;
        
        public AllyIdleState(GameUnit unit, IStateSwitcher stateSwitcher, IBuildingManager buildingManager,
            IUnitHolder unitHolder) : base(unit, stateSwitcher)
        {
            this.buildingManager = buildingManager;
            this.unitHolder = unitHolder;
            
            _allyGameUnitToControl = unit as AllyGameUnit;
        }

        public override void OnStateEnter()
        {
            nextTickTime = float.MinValue;
        }

        public override void OnStateExit()
        {
            nextTickTime = float.MaxValue;
        }

        public override void OnStateUpdate(float deltaTime)
        {
            if (Time.time < nextTickTime)
            {
                return;
            }

            nextTickTime = Time.time + TickDuraction;
            
            if (EnemyIsNear()) //TODO: think about adding conditions 
            {
                _allyGameUnitToControl.SetTarget(_closestEnemyGame);
                stateSwitcher.SwitchState<AllyMoveToTargetUnitState>();
                return;
            }
            
            if (NeedToHeal())
            {
                _allyGameUnitToControl.SetPointToMove(buildingManager.GetSanctumPosition());
                stateSwitcher.SwitchState<AllyMoveToPointUnitState>();
                return;
            }
            
            if (FarFromTower())
            {
                _allyGameUnitToControl.SetPointToMove(buildingManager.GetMainTowerPosition());
                stateSwitcher.SwitchState<AllyMoveToPointUnitState>();
                return;
            }
        }

        private bool FarFromTower()
        {
            Vector3 mainTowerPosition = buildingManager.GetMainTowerPosition();
            Vector3 currentPosition = UnitToControl.GetPosition();
            float distanceToTower = Vector3.Distance(currentPosition, mainTowerPosition);
            return distanceToTower > UnitToControl.GetSearchRadius();
        }

        private bool EnemyIsNear()
        {
            Vector3 currentPosition = UnitToControl.GetPosition();
            if (unitHolder.TryGetGlosestUnit(UnitFaction.Enemy, currentPosition, out _closestEnemyGame) == false)
            {
                return false;
            }
            
            float distanceToEnemy = Vector3.Distance(currentPosition, _closestEnemyGame.GetPosition());
            
            return distanceToEnemy < UnitToControl.GetSearchRadius();
        }

        private bool NeedToHeal()
        {
            return UnitToControl.GetCurrentHealth() < UnitToControl.GetMaxHealth() * HPAllowedThreshold;
        }
    }
}
