using GameSceneObjects.Units;
using UnityEngine;
using UnityEngine.AI;

namespace GameSceneObjects.StateBehaviour.HeroStates
{
    public class HeroMoveToPointUnitState : BaseUnitState
    {
        private const float TickPeriod = 1f;
        
        protected NavMeshAgent controllableAgent;
    
        private float nextTickTime = float.MinValue;
        private float  stopDistance = 1f;
        
        public HeroMoveToPointUnitState(GameUnit unit, IStateSwitcher stateSwitcher) : base(unit, stateSwitcher)
        {
            controllableAgent = unit.GetNavMeshAgent();
            stopDistance = unit.GetUnitStopDistance();
        }

        public override void OnStateEnter()
        {
            unitToControl.ChangeAnimatorState(GameUnit.WalkAnimParameter, true);
        }

        public override void OnStateExit()
        {
            unitToControl.ChangeAnimatorState(GameUnit.WalkAnimParameter, false);
        }

        public override void OnStateUpdate(float deltaTime)
        {
            if (Time.time < nextTickTime)
            {
                return;
            }
            
            nextTickTime = Time.time + TickPeriod;
            
            if (Vector3.Distance(unitToControl.transform.position, controllableAgent.destination) < stopDistance)
            {
                stateSwitcher.SwitchState<HeroCalmUnitState>();
            }
        }
    }
}
