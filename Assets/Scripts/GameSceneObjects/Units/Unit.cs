using PoolingSystem;
using UnityEngine;
using UnityEngine.AI;

namespace GameSceneObjects.Units
{
    public abstract class Unit : MonoBehaviour, IHittable, IPoolable
    {
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] protected UnitInfo info;
        
        protected int currentLevel = 1;
        private float currentHealth;
        
        public virtual void Init()
        {
            currentHealth = info.Health;
            navMeshAgent.speed = info.MovementSpeed;
        }

        public void GetDamage(float dmg)
        {
            dmg *= (1f - info.Armor);
            currentHealth -= dmg;
            
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDie();
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

        public float GetAttackRange()
        {
            return info.AttackRange;
        }

        public NavMeshAgent GetNavMeshAgent()
        {
            return navMeshAgent;
        }

        public float GetAttackCooldown()
        {
            return 1f / info.AttackSpeed;
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public float GetMaxHealth()
        {
            return info.Health;
        }

        public double GetSearchRadius()
        {
            return info.SearchRange;
        }

        public float GetUnitStopDistance()
        {
            return info.UnitStopDistance;
        }

        public virtual void OnDie()
        {
            gameObject.SetActive(false);
        }
        
        public UnitFaction GetFaction()
        {
            return info.Faction;
        }

        public void OnReturnToPool()
        {
            navMeshAgent.enabled = false;
        }

        public void OnGetFromPool()
        {
            navMeshAgent.enabled = true;
        }
    }
}
