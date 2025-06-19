using GameSceneObjects.Buildings;
using GameSceneObjects.StateBehaviour;
using Services;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class AllyGameUnit : GameUnit, IWithTarget, IClickInteractable, ISanctumable
    {
        [SerializeField] private GameObject selectionMark;
        
        private IBuildingManager buildingManager;
        private IUnitHolder unitHolder;
        
        private IHittable currentTarget;
        private AllyStationBehaviour stationBehaviour;

        private Vector3 lastPointOfInterest;
        
        private bool isSanctumActive = false;
        
        public void SetTarget(IHittable target)
        {
            currentTarget = target;
            if (selectionMark.activeSelf)
            {
                currentTarget.ShowTargetPointer();
            }
        }

        public IHittable GetCurrentTarget()
        {
            return currentTarget;
        }

        public bool HasTarget()
        {
            return currentTarget != null;
        }

        public override bool CanBeAttacked()
        {
            return base.CanBeAttacked() && isSanctumActive == false;
        }
        
        public void InflictDamage()
        {
            if (HasTarget() == false || currentTarget.IsAlive() == false)
            {
                return;
            }
            
            float damage = UnitDamage();
            currentTarget.GetDamage(damage);
        }
        
        public void OnSelect()
        {
            selectionMark.SetActive(true);
            currentTarget?.ShowTargetPointer();
        }

        public void OnDeselect()
        {
            selectionMark.SetActive(false);
        }

        public void InteractWithUnit(GameUnit unt)
        {
            if (unt is EnemyGameUnit enemy)
            {
                SetTarget(unt);
                float distanceToTarget = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToTarget <= GetAttackRange())
                {
                    stationBehaviour.SwitchState<AllyAttackTargetUnitState>();
                }
                else
                {
                    stationBehaviour.SwitchState<AllyMoveToTargetUnitState>();
                }
                return;
            }
            
            MoveTo(unt.transform.position);
        }

        public void MoveTo(Vector3 targetPosition)
        {
            if (IsAlive() == false)
            {
                return;
            }
            
            SetPointToMove(targetPosition);
            stationBehaviour.SwitchState<AllyMoveToPointUnitState>();
        }
        
        public void SetPointToMove(Vector3 movePoint)
        {
            lastPointOfInterest = movePoint;
        }
        
        public Vector3 GetLastInterestPoint()
        {
             return lastPointOfInterest;
        }

        public void SetNavigationPoint(Vector3 pointToMove)
        {
            navMeshAgent.SetDestination(pointToMove);
        }
        
        public override void OnDie()
        {
            base.OnDie();
            unitHolder.UnregisterUnit(this);
        }
        
        protected override void Start()
        {
            base.Start();
            
            buildingManager = ServiceLocator.Instance.Get<IBuildingManager>();
            unitHolder = ServiceLocator.Instance.Get<IUnitHolder>();
            
            stationBehaviour = new AllyStationBehaviour(this, unitHolder, buildingManager, effectHolder);
            stationBehaviour.SwitchState<AllyIdleState>();
        }
        
        private void Update()
        {
            stationBehaviour.OnUpdate(Time.deltaTime);
        }
        
        public override void Heal(float healEffect)
        {
            if (IsAlive() == false)
            {
                return;
            }
            
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
    }
}
