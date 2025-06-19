using GameSceneObjects.Units;
using UnityEngine;
using UnityEngine.AI;

namespace GameSceneObjects.StateBehaviour
{
    public class AllyMoveToPointUnitState : BaseUnitState
    {
        private const float NavMeshPointSearchRadius = 10f;
        
        private float TickDuraction = 1f;
        
        private AllyGameUnit allyGameUnitToControl;
        private float approachDistance = float.MaxValue;
        
        private float nextTickTime = float.MinValue;
        private Vector3 pointToMove;
        
        public AllyMoveToPointUnitState(GameUnit unit, IStateSwitcher stateSwitcher) : base(unit, stateSwitcher)
        {
            allyGameUnitToControl = unit as AllyGameUnit;
            approachDistance = unit.GetUnitStopDistance();
        }

        public override void OnStateEnter()
        {
            pointToMove = allyGameUnitToControl.GetLastInterestPoint();
            
            if (NavMesh.SamplePosition(pointToMove, out NavMeshHit hit, NavMeshPointSearchRadius, NavMesh.AllAreas))
            {
                pointToMove = hit.position;
                unitToControl.ChangeAnimatorState(GameUnit.WalkAnimParameter, true);
                allyGameUnitToControl.SetNavigationPoint(pointToMove);
                nextTickTime = Time.time + TickDuraction;
                return;
            }
            
            stateSwitcher.SwitchState<AllyIdleState>();
        }
        
        public override void OnStateExit()
        {
            unitToControl.ChangeAnimatorState(GameUnit.WalkAnimParameter, false);
            nextTickTime = float.MaxValue;
        }

        public override void OnStateUpdate(float deltaTime)
        {
            if (Time.time < nextTickTime)
            {
                return;
            }

            nextTickTime = Time.time + TickDuraction;
            
            if (Vector3.Distance(allyGameUnitToControl.transform.position, allyGameUnitToControl.GetLastInterestPoint()) 
                <= approachDistance)
            {
                stateSwitcher.SwitchState<AllyIdleState>();
            }
        }
    }
}
