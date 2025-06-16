using GameSceneObjects.HeroManagement;
using GameSceneObjects.StateBehaviour;
using GameSceneObjects.StateBehaviour.HeroStates;
using Services;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class Hero : GameUnit, IClickInteractable, ISanctumable, IWithTarget
    {
        private IHeroManager heroManager;
        
        private HeroStationBehaviour stationBehaviour;
        
        private IHittable currentTarget;
        private bool isSanctumActive = false;
        
        public bool IsSanctumActive()
        {
            return isSanctumActive;
        }
        
        public override void Init()
        {
            base.Init();
            heroManager = ServiceLocator.Instance.Get<IHeroManager>();
        }
        
        public override void GetDamage(float dmg)
        {
            base.GetDamage(dmg);
            
            heroManager.OnHPChange();   
        }
        
        public float GetHealthRatio()
        {
            return currentHealth / info.Health;
        }
        
        public override void OnDie()
        {
            heroManager.OnHeroDie(this);
            base.OnDie();
        }

        public void OnSelect()
        {
            Debug.Log("Hero selected! " + gameObject.name);
        }

        public void OnDeselect()
        {
            Debug.Log("Hero deselected! " + gameObject.name);
        }

        public void InteractWithUnit(GameUnit unt)
        {
            if (unt is EnemyGameUnit enemy)
            {
                SetTarget(enemy);
                stationBehaviour.SwitchState<HeroMoveToTargetUnitState>();
            }
        }

        public void MoveTo(Vector3 targetPosition)
        {
            stationBehaviour.SwitchState<HeroMoveToTargetUnitState>();
            navMeshAgent.SetDestination(targetPosition);
        }
        
        public override bool CanBeAttacked()
        {
            return base.CanBeAttacked() && isSanctumActive == false;
        }
        
        public override void Heal(float healEffect)
        {
            heroManager.OnHPChange(); //TODO: check ingame behaviour on incative hero near sancrtum
            
            base.Heal(healEffect);
        }

        public void OnSanctumeEnter()
        {
            isSanctumActive = true;
        }

        public void OnSanctumExit()
        {
            isSanctumActive = false;
        }
        
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
            float damage = info.AttackPower * currentLevel; //TODO: add an animation curve to calculate damage
            currentTarget.GetDamage(damage);
        }
        
        protected override void Start()
        {
            base.Start();
            stationBehaviour = new HeroStationBehaviour(this, heroManager);
        }
        
        private void Update()
        {
            stationBehaviour.OnUpdate(Time.deltaTime);
        }
    }
}
