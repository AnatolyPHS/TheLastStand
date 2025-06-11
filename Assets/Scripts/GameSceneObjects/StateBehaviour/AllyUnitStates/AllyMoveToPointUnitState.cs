using GameSceneObjects.Units;
using UnityEngine;
using UnityEngine.AI;

namespace GameSceneObjects.StateBehaviour
{
    public class AllyMoveToPointUnitState : BaseUnitState
    {
        private const float NavMeshPointSearchRadius = 10f;
        
        private float TickDuraction = 1f;
        
        private AllyUnit allyUnitToControl;
        private float approachDistance = float.MaxValue;
        
        private float nextTickTime = float.MinValue;
        private Vector3 pointToMove;
        
        public AllyMoveToPointUnitState(Unit unit, IStateSwitcher stateSwitcher) : base(unit, stateSwitcher)
        {
            allyUnitToControl = unit as AllyUnit;
            approachDistance = unit.GetUnitStopDistance();
        }

        public override void OnStateEnter()
        {
            pointToMove = allyUnitToControl.GetLastInterestPoint();
            if (NavMesh.SamplePosition(pointToMove, out NavMeshHit hit, NavMeshPointSearchRadius, NavMesh.AllAreas))
            {
                pointToMove = hit.position;
                allyUnitToControl.SetNavigationPoint(pointToMove);
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
            
            if (Vector3.Distance(allyUnitToControl.transform.position, allyUnitToControl.GetLastInterestPoint()) 
                <= approachDistance)
            {
                stateSwitcher.SwitchState<AllyIdleState>();
            }
        }
    }
}
