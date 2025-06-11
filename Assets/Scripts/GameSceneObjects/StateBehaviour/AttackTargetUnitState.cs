using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.StateBehaviour
{
    public class AttackTargetUnitState : BaseUnitState
    {
        private IWithTarget attacker;
    
        private float nextAttackTime = float.MinValue;
        private float attackCooldown = float.MaxValue;
    
        public AttackTargetUnitState(Unit unit, StationBehaviour stationBehaviour) 
            : base(unit, stationBehaviour)
        {
            attackCooldown = unit.GetAttackCooldown();
            attacker = unit as IWithTarget;
        }

        public override void OnStateEnter()
        {
            ProcessAttack();
            nextAttackTime = Time.time + attackCooldown;
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
        
            nextAttackTime = Time.time + attackCooldown;
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
            if (CloseToAttack(target))
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

        private bool CloseToAttack(IHittable target)
        {
            return Vector3.Distance(unitToControl.transform.position, target.GetPosition()) <= unitToControl.GetAttackRange();
        }
    }
}
