using EffectsManager;
using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemyAttackTargetUnitState : AttackTargetUnitState
    {
        public EnemyAttackTargetUnitState(GameUnit unit, IEffectHolder effectManager, StationBehaviour stationBehaviour) : base(unit, effectManager, stationBehaviour)
        {
        }
        
        protected override void SwitchToMoveToTargetState()
        {
            stateSwitcher.SwitchState<EnemyMoveToTargetUnitState>();
        }

        protected override void SwitchToIdleSate()
        {
            stateSwitcher.SwitchState<EnemySearchForTargetUnitState>();
        }

        protected override bool CanAttack(IHittable target)
        {
            bool inSanctum = target is ISanctumable sanctum && sanctum.IsSanctumActive();
            return inSanctum == false && base.CanAttack(target);
        }
    }
}
