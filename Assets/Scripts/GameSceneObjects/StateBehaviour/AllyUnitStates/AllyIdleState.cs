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

        private AllyUnit allyUnitToControl;
        private float nextTickTime = float.MinValue;

        private EnemyUnit closestEnemy;
        
        public AllyIdleState(Unit unit, IStateSwitcher stateSwitcher, IBuildingManager buildingManager,
            IUnitHolder unitHolder) : base(unit, stateSwitcher)
        {
            this.buildingManager = buildingManager;
            this.unitHolder = unitHolder;
            
            allyUnitToControl = unit as AllyUnit;
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
                allyUnitToControl.SetTarget(closestEnemy);
                stateSwitcher.SwitchState<AllyMoveToTargetUnitState>();
                return;
            }
            
            if (NeedToHeal())
            {
                allyUnitToControl.SetPointToMove(buildingManager.GetSanctumPosition());
                stateSwitcher.SwitchState<AllyMoveToPointUnitState>();
                return;
            }
            
            if (FarFromTower())
            {
                allyUnitToControl.SetPointToMove(buildingManager.GetMainTowerPosition());
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
            if (unitHolder.TryGetGlosestEnemy(currentPosition, out closestEnemy) == false)
            {
                return false;
            }
            
            float distanceToEnemy = Vector3.Distance(currentPosition, closestEnemy.GetPosition());
            
            return distanceToEnemy < unitToControl.GetSearchRadius();
        }

        private bool NeedToHeal()
        {
            return unitToControl.GetCurrentHealth() < unitToControl.GetMaxHealth() * HPAllowedThreshold;
        }
    }
}
