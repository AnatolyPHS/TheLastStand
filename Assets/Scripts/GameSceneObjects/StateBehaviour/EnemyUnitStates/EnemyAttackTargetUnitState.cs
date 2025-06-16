using GameSceneObjects.Buildings;
using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class EnemyAttackTargetUnitState : AttackTargetUnitState
    {
        public EnemyAttackTargetUnitState(GameUnit gameUnit, StationBehaviour stationBehaviour) : base(gameUnit, stationBehaviour)
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
