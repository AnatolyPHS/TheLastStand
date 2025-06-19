using System;
using GameSceneObjects.Data.AbilityData;
using GameSceneObjects.HeroManagement.Abilities;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public enum AbilityType
    {
        None = 0,
        MeteorShower = 10,
        FreezingArrow = 20,
    }
    
    [Serializable]
    public abstract class AbilityBase
    {
        protected AbilityController abilityController;
        
        protected int abiliyLevel = 1;
        protected float lastTimeUsed = float.MinValue;
        
        protected bool abilityInUse = false;

        private float cooldown = float.MinValue;
        private AbilityType abilityType;
        
        public AbilityType GetAbilityType => abilityType;
        
        public AbilityBase(AbilityBaseInfo abilityBaseInfo, AbilityController controller)
        {
            abilityType = abilityBaseInfo.AbilityType;
            abilityController = controller;
            cooldown = abilityBaseInfo.CooldownTime;
        }

        public virtual void AbilityButtonClick(float f)
        {
            abilityController.SetCurrentAbilityType(GetAbilityType);
            abilityInUse = true;
        }
        public abstract void OnUpdate(Vector3 pointer);
        public abstract void ActivateAbility(Vector3 mouseGroundPosition);

        public void UpgradeAbility()
        {
            abiliyLevel++;
            Debug.Log(abiliyLevel);
        }
        
        protected bool InCooldown()
        {
            return Time.time - lastTimeUsed < cooldown;
        }
    }
}
