using System.Collections.Generic;
using Currencies;
using GameSceneObjects.HeroManagement;
using GameSceneObjects.StateBehaviour;
using GameSceneObjects.Units.Buffs;
using Services;
using UnityEngine;

namespace GameSceneObjects.Units
{
    public class EnemyGameUnit : GameUnit, IWithTarget
    {
        private List<BuffBase> buffs = new List<BuffBase>();
        
        private IHeroManager heroManager;
        private IUnitHolder unitHolder;
        private ICurrencyTracker currencyTracker;
        
        private EnemyStationBehaviour stationBehaviour;
        
        private IHittable currentTarget;
        private float attackSpeedFactor = 1f;
        
        public float AttackSpeedFactor
        {
            get => attackSpeedFactor;
            set => attackSpeedFactor = value;
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
            float damage = CalculateDamage();
            currentTarget.GetDamage(damage);
        }

        public override float GetAttackCooldown()
        {
            return  (1f / info.AttackSpeed) / attackSpeedFactor;
        }
        
        public override void OnDie()
        {
            base.OnDie();
            
            unitHolder.UnregisterUnit(this);
            currencyTracker.ChangeCurrencyValue(GetCost());
            heroManager.AddExperience(GetCost()); //TODO: check if the hero killed it
        }
        
        public void AddBuff(UnitBuffType buff, float power, float freezeDuration) //TODO: spread the buff logic to all unit types and buffs
        {
            if (buff != UnitBuffType.Slow)
            {
                Debug.LogError($"Buff {buff} is not implemented for {nameof(EnemyGameUnit)}");
                return;
            }

            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i].GetBuffType == buff)
                {
                    buffs[i].RefreshBuff(freezeDuration, power);
                    return; 
                }
            }
            
            SlowDebuff slowDebuff = new SlowDebuff();
            slowDebuff.OnImplement(this, freezeDuration, power);
            buffs.Add(slowDebuff);
        }

        public void RemoveBuff(UnitBuffType buffType)
        {
            buffs.RemoveAll(buff => buff.GetBuffType == buffType);
        }
        
        protected override void Start()
        {
            base.Start();
            
            heroManager = ServiceLocator.Instance.Get<IHeroManager>();
            unitHolder = ServiceLocator.Instance.Get<IUnitHolder>();
            currencyTracker = ServiceLocator.Instance.Get<ICurrencyTracker>();
            
            stationBehaviour = new EnemyStationBehaviour(this, heroManager);
            stationBehaviour.SwitchState<EnemySearchForTargetUnitState>();
        }

        private void Update()
        {
            stationBehaviour.OnUpdate(Time.deltaTime);
            UpdateDebuffs();
        }

        private void UpdateDebuffs()
        {
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                BuffBase bf = buffs[i];
                if (Time.time >= bf.EndTime)
                {
                    bf.OnRemoveDebuff(this);
                    buffs.RemoveAt(i);
                }
            }
        }

        private float CalculateDamage()
        {
            return info.AttackPower * currentLevel; //TODO: add an animation curve to calculate damage
        }
    }
}
