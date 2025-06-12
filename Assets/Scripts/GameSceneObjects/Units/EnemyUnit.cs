using GameSceneObjects.HeroManagement;
using GameSceneObjects.StateBehaviour;
using Services;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class EnemyUnit : Unit, IWithTarget
    {
        private IHeroManager heroManager;
        private IUnitHolder unitHolder;
        
        private EnemyStationBehaviour stationBehaviour;
        
        private IHittable currentTarget;
        
        public void SetTarget(IHittable target)
        {
            currentTarget = target;
        }
        
        public IHittable GetCurrentTarget()
        {
            return currentTarget;
        }

        public bool HasTarget()
        {
            return currentTarget != null;
        }

        public void InflictDamage()
        {
            float damage = CalculateDamage();
            currentTarget.GetDamage(damage);
        }

        public override void OnDie()
        {
            base.OnDie();
            unitHolder.UnregisterUnit(this);
        }
        
        private void Start()
        {
            heroManager = ServiceLocator.Instance.Get<IHeroManager>();
            unitHolder = ServiceLocator.Instance.Get<IUnitHolder>();
            
            stationBehaviour = new EnemyStationBehaviour(this, heroManager);
            stationBehaviour.SwitchState<EnemySearchForTargetUnitState>();
        }

        private void Update()
        {
            stationBehaviour.OnUpdate(Time.deltaTime);
        }

        private float CalculateDamage()
        {
            return info.AttackPower * currentLevel; //TODO: add an animation curve to calculate damage
        }
    }
}
