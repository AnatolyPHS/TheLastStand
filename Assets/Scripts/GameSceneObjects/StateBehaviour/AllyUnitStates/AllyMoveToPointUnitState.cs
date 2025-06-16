using GameSceneObjects.Units;
using UnityEngine;
using UnityEngine.AI;

namespace GameSceneObjects.StateBehaviour
{
    public class AllyMoveToPointUnitState : BaseUnitState
    {
        private const float NavMeshPointSearchRadius = 10f;
        
        private float TickDuraction = 1f;
        
        private AllyGameUnit _allyGameUnitToControl;
        private float approachDistance = float.MaxValue;
        
        private float nextTickTime = float.MinValue;
        private Vector3 pointToMove;
        
        public AllyMoveToPointUnitState(GameUnit gameUnit, IStateSwitcher stateSwitcher) : base(gameUnit, stateSwitcher)
        {
            _allyGameUnitToControl = gameUnit as AllyGameUnit;
            approachDistance = gameUnit.GetUnitStopDistance();
        }

        public override void OnStateEnter()
        {
            pointToMove = _allyGameUnitToControl.GetLastInterestPoint();
            if (NavMesh.SamplePosition(pointToMove, out NavMeshHit hit, NavMeshPointSearchRadius, NavMesh.AllAreas))
            {
                pointToMove = hit.position;
                _allyGameUnitToControl.SetNavigationPoint(pointToMove);
                nextTickTime = Time.time + TickDuraction;
                return;
            }
            
            stateSwitcher.SwitchState<AllyIdleState>();
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
            
            if (Vector3.Distance(_allyGameUnitToControl.transform.position, _allyGameUnitToControl.GetLastInterestPoint()) 
                <= approachDistance)
            {
                stateSwitcher.SwitchState<AllyIdleState>();
            }
        }
    }
}
