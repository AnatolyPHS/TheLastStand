using GameSceneObjects.HeroManagement;
using Services;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class Hero : Unit, IClickInteractable, ISanctumable
    {
        private IHeroManager heroManager;
        
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

        public void InteractWithUnit(Unit unt)
        {
            Debug.Log("Hero interacting with unit: " + unt.gameObject.name);
        }

        public void MoveTo(Vector3 targetPosition)
        {
            navMeshAgent.SetDestination(targetPosition);
        }
        
        public override bool CanBeAttacked()
        {
            return base.CanBeAttacked() && isSanctumActive == false;
        }
        
        public void Heal(float healEffect)
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
