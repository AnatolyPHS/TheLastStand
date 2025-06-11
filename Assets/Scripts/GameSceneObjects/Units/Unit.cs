using UnityEngine;
using UnityEngine.AI;

namespace GameSceneObjects.Units
{
    public abstract class Unit : MonoBehaviour, IHittable
    {
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] protected UnitInfo info;
        
        protected int currentLevel = 1;
        private float currentHealth;

        private void Start()
        {
            Init(); //TODO: tmp measure, remove after implementing pooling or spawning system
        }
        
        public virtual void Init()
        {
            currentHealth = info.Health;
            navMeshAgent.speed = info.MovementSpeed;
        }

        public void GetDamage(float dmg)
        {
            dmg *= (1f - info.Armor);
            currentHealth -= dmg;
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
    }
}
