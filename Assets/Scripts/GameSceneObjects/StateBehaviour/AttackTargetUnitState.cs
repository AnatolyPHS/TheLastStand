using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.StateBehaviour
{
    public class AttackTargetUnitState : BaseUnitState
    {
        private IWithTarget attacker;
    
        private float nextAttackTime = float.MinValue;
    
        public AttackTargetUnitState(GameUnit unit, StationBehaviour stationBehaviour) 
            : base(unit, stationBehaviour)
        {
            attacker = unit as IWithTarget;
        }

        public override void OnStateEnter()
        {
            ProcessAttack();
            nextAttackTime = Time.time + unitToControl.GetAttackCooldown();
        }

        public override void OnStateExit()
        {
            nextAttackTime = float.MaxValue;
        }

        public override void OnStateUpdate(float deltaTime)
        {
            if (Time.time < nextAttackTime)
            {
                return;
            }
        
            nextAttackTime = Time.time + unitToControl.GetAttackCooldown();
            ProcessAttack();
        }

        private void ProcessAttack()
        {
            if (attacker.HasTarget() == false || attacker.GetCurrentTarget().IsAlive() == false)
            {
                SwitchToIdleSate();
                return;
            }

            IHittable target = attacker.GetCurrentTarget();
            if (CanAttack(target))
            {
                attacker.InflictDamage();
            }
            else
            {
                SwitchToMoveToTargetState();
            }
        }

        protected virtual void SwitchToMoveToTargetState()
        {
            stateSwitcher.SwitchState<MoveToTargetUnitState>();
        }

        protected virtual void SwitchToIdleSate()
        {
            stateSwitcher.SwitchState<SearchForTargetUnitState>();
        }

        protected virtual bool CanAttack(IHittable target)
        {
            return Vector3.Distance(unitToControl.transform.position, target.GetPosition()) <= unitToControl.GetAttackRange();
        }
    }
}
