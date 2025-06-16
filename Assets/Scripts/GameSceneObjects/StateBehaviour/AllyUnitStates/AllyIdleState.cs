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

        private GameUnit closestTarget;
        
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
                _allyGameUnitToControl.SetTarget(closestTarget);
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
            Vector3 currentPosition = unitToControl.GetPosition();
            float distanceToTower = Vector3.Distance(currentPosition, mainTowerPosition);
            return distanceToTower > unitToControl.GetSearchRadius();
        }

        private bool EnemyIsNear()
        {
            Vector3 currentPosition = unitToControl.GetPosition();
            if (unitHolder.TryGetGlosestUnit(UnitFaction.Enemy, currentPosition, out closestTarget) == false)
            {
                return false;
            }
            
            float distanceToEnemy = Vector3.Distance(currentPosition, closestTarget.GetPosition());
            
            return distanceToEnemy < unitToControl.GetSearchRadius();
        }

        private bool NeedToHeal()
        {
            return unitToControl.GetCurrentHealth() < unitToControl.GetMaxHealth() * HPAllowedThreshold;
        }
    }
}
