using EffectsManager;
using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.StateBehaviour
{
    public class AttackTargetUnitState : BaseUnitState
    {
        private IEffectHolder effectManager;
        
        private IWithTarget attacker;
    
        private float nextAttackTime = float.MinValue;
    
        public AttackTargetUnitState(GameUnit unit, IEffectHolder effectManager, StationBehaviour stationBehaviour) 
            : base(unit, stationBehaviour)
        {
            attacker = unit as IWithTarget;
            this.effectManager = effectManager;
        }

        public override void OnStateEnter()
        {
            ProcessAttack();
            nextAttackTime = Time.time + unitToControl.GetAttackCooldown();
            RotateToTarget();
            unitToControl.ChangeAnimatorState(GameUnit.Attack01AnimParameter, true);
        }
        
        public override void OnStateExit()
        {
            nextAttackTime = float.MaxValue;
            unitToControl.ChangeAnimatorState(GameUnit.Attack01AnimParameter, false);
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
                RotateToTarget();
                PlayAttackEffect(target);
                attacker.InflictDamage();
            }
            else
            {
                SwitchToMoveToTargetState();
            }
        }

        private void PlayAttackEffect(IHittable target)
        {
            EffectType attackEffectType = unitToControl.GetAttackEffectType();

            if (attackEffectType == EffectType.None)
            {
                return;
            }
            
            Vector3 effectPosition = unitToControl.transform.position + Vector3.up * 0.5f;
            effectManager.ShootEffect(attackEffectType, effectPosition, target.GetPosition());
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
        
        private void RotateToTarget()
        {
            Vector3 targetPosition = attacker.GetCurrentTarget().GetPosition();
            unitToControl.transform.LookAt(new Vector3(targetPosition.x, unitToControl.transform.position.y, targetPosition.z));
        }
    }
}
