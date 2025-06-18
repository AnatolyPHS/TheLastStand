using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.StateBehaviour
{
    public class SearchForTargetUnitState : BaseUnitState
    {
        private const float TickPeriod = 1f;
        
        private float nextTickTime = float.MinValue;
        
        public SearchForTargetUnitState(GameUnit unit, IStateSwitcher stateSwitcher) : base(unit, stateSwitcher)
        {
        }
        
        public override void OnStateEnter()
        {
            ProcessLookForTarget();
            nextTickTime = Time.time + TickPeriod;
            unitToControl.ChangeAnimatorState(GameUnit.Idle01AnimParameter, true);
        }
        
        public override void OnStateExit()
        {
            nextTickTime = float.MaxValue;
            unitToControl.ChangeAnimatorState(GameUnit.Idle01AnimParameter, true);
        }

        public override void OnStateUpdate(float deltaTime)
        {
            if (Time.time < nextTickTime)
            {
                return;
            }
            
            nextTickTime = Time.time + TickPeriod;
            ProcessLookForTarget();
        }
        
        protected virtual void ProcessLookForTarget()
        {
        }
    }
}
