using UnityEngine;

namespace GameSceneObjects.Buildings
{
    public class Сitadel : BuildingBase, IHittable
    {
        [SerializeField] private float maxHealth = 1000f;
        
        private float currentHealth;
        
        public void GetDamage(float dmg)
        {
            currentHealth -= dmg;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                //TODO: show gameover screen
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

        private void Start()
        {
            currentHealth = maxHealth;
        }
    }
}
