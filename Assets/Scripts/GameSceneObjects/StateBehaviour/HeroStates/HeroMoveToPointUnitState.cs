using GameSceneObjects.Units;
using UnityEngine;
using UnityEngine.AI;

namespace GameSceneObjects.StateBehaviour.HeroStates
{
    public class HeroMoveToPointUnitState : BaseUnitState
    {
        private const float  StopDistance = 1f;
        private const float TickPeriod = 1f;
        
        protected NavMeshAgent controllableAgent;
    
        private float nextTickTime = float.MinValue;
        
        public HeroMoveToPointUnitState(GameUnit unit, IStateSwitcher stateSwitcher) : base(unit, stateSwitcher)
        {
            controllableAgent = unit.GetNavMeshAgent();
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
            
            if (Vector3.Distance(unitToControl.transform.position, controllableAgent.destination) < StopDistance)
            {
                stateSwitcher.SwitchState<HeroCalmUnitState>();
            }
        }
    }
}
