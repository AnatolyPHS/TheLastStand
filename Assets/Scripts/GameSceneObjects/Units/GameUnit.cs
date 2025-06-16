using EffectsManager;
using PoolingSystem;
using Services;
using UnityEngine;
using UnityEngine.AI;

namespace GameSceneObjects.Units
{
    public enum UnitBuffType
    {
        None = 0,
        Slow = 10,
    }
    
    public abstract class GameUnit : MonoBehaviour, IHittable, IPoolable
    {
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] protected UnitInfo info;
        
        private IEffectHolder effectHolder;
        private IPoolManager poolManager;
        
        protected int currentLevel = 1;
        protected float levelFactor = 1f;
        
        protected float currentHealth;
        protected float attackRange;
        protected float attackCooldown;
        protected float attackDamage;
        protected int unitCost;
        private float maxHealth;
        private float armor;

        public virtual void SetLevel(int level)
        {
            currentLevel = level;
            levelFactor = info.GetLVLFactor(currentLevel);
        }
        
        public virtual void Init()
        {
            ResetUnitStats();
            currentHealth = GetMaxHealth();
        }
        
        public virtual void GetDamage(float dmg)
        {
            dmg *= (1f - CalculateArmor());
            currentHealth -= dmg;
            
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDie();
            }
        }

        private float CalculateArmor() 
        {
            return armor;
        }

        public bool IsAlive()
        {
            return currentHealth > 0;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public virtual void Heal(float healEffect)
        {
            if (IsAlive() == false)
            {
                return;
            }
            
            float maxHealth = GetMaxHealth();
            currentHealth += healEffect * maxHealth;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        public virtual bool CanBeAttacked()
        {
            return IsAlive();
        }

        public void ShowTargetPointer()
        {
            effectHolder.PlayEffect(EffectType.EnemyPointer, transform.position, Quaternion.identity, transform);
        }

        public float GetAttackRange()
        {
            return attackRange;
        }

        public NavMeshAgent GetNavMeshAgent()
        {
            return navMeshAgent;
        }

        public virtual float GetAttackCooldown()
        {
            return attackCooldown;
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
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
            poolManager.ReturnObject(this);
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

        protected virtual void Start()
        {
            poolManager = ServiceLocator.Instance.Get<IPoolManager>();
            effectHolder = ServiceLocator.Instance.Get<IEffectHolder>();
        }

        public int GetCost()
        {
            return unitCost;
        }

        public Sprite GetIcon()
        {
            return info.UnitIcon;
        }
        
        protected float UnitDamage()
        {
            return attackDamage;
        }
        
        protected void ResetUnitStats()
        {
            maxHealth = info.Health * levelFactor;
            attackRange = info.AttackRange * levelFactor;
            attackDamage = info.AttackPower * levelFactor;
            unitCost = (int) (info.UnitCost * levelFactor);
            attackCooldown = 1f / (info.AttackSpeed * levelFactor);
            armor = Mathf.Clamp(info.Armor * (1 + levelFactor / info.GetMaxLvlFactor()), 0, 0.99f); //TODO: think about asymptotic  growth formula
            navMeshAgent.speed = info.MovementSpeed * levelFactor;
        }
    }
}
