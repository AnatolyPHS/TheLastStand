using Services;
using UI.GameView;
using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class Сitadel : BuildingBase, IHittable
    {
        [SerializeField] private float maxHealth = 1000f;
     
        private IEndGameViewController endGameViewController;
        
        private float currentHealth;
        
        public void GetDamage(float dmg)
        {
            currentHealth -= dmg;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                endGameViewController.ShowEndGameView("The citadel has been destroyed! Game Over!");
            }
        }

        public bool IsAlive()
        {
            return currentHealth > 0;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void Heal(float healEffect)
        {
            currentHealth += healEffect;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        public bool CanBeAttacked()
        {
            return IsAlive();
        }

        public void ShowTargetPointer()
        {
            //TODO: implement target pointer after adding an ability to repair the citadel
        }

        private void Start()
        {
            currentHealth = maxHealth;
            endGameViewController = ServiceLocator.Instance.Get<IEndGameViewController>();
        }
    }
}
