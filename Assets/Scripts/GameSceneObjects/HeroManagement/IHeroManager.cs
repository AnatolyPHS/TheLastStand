using GameEvents;
using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public interface IHeroManager : IEventTrigger<HeroChangeState, HeroChangeStateEventData>
    {
        Hero GetHero();
        void OnHeroDie(Hero hero);
        Vector3 GetHeroPosition();
        bool HeroIsRespawning();
        void OnHeroRespawn(GameUnit gameUnit);
        void AddExperience(int experience);
        void OnHPChange();
        int GetHeroLevel();
        void OnAbilityUpgradeClick(AbilityType abilityType);
        bool IsCastingSpell();
    }

    public struct HeroChangeStateEventData
    {
        public float nextLevelRatio;
        public float skillPoints;
        public float hpRatio;
        public int heroLvl;
    }

    public class HeroChangeState : BaseEvent
    {
    }
}