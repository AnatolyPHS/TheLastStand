using GameSceneObjects.Buildings;
using GameSceneObjects.StateBehaviour;
using Services;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class AllyUnit : Unit, IWithTarget, IClickInteractable
    {
        [SerializeField] private GameObject selectionMark;
        
        private IBuildingManager buildingManager;
        private IUnitHolder unitHolder;
        
        private IHittable currentTarget;
        private AllyStationBehaviour stationBehaviour;

        private Vector3 lastPointOfInterest;
        
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
            if (HasTarget() == false || currentTarget.IsAlive() == false)
            {
                return;
            }
            
            float damage = CalculateDamage();
            currentTarget.GetDamage(damage);
            Debug.Log("Ally is attacking ");
        }
        
        public void OnSelect()
        {
            selectionMark.SetActive(true);
        }

        public void OnDeselect()
        {
            selectionMark.SetActive(false);
        }

        public void InteractWithUnit(Unit unt)
        {
            if (unt is EnemyUnit)
            {
                SetTarget(unt);
                stationBehaviour.SwitchState<AllyMoveToTargetUnitState>();
                return;
            }
            
            MoveTo(unt.transform.position);
        }

        public void MoveTo(Vector3 targetPosition)
        {
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
            
            stationBehaviour = new AllyStationBehaviour(this, unitHolder, buildingManager);
            stationBehaviour.SwitchState<AllyIdleState>();
        }
        
        private void Update()
        {
            stationBehaviour.OnUpdate(Time.deltaTime);
        }
        
        private float CalculateDamage()
        {
            return info.AttackPower * currentLevel; //TODO: add an animation curves to calculate damage
        }
    }
}
