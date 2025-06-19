using System;
using System.Collections.Generic;
using Camera;
using GameEvents;
using GameSceneObjects.Buildings;
using GameSceneObjects.Data.AbilityData;
using GameSceneObjects.HeroManagement.Abilities;
using GameSceneObjects.Units;
using InputsManager;
using Selector;
using Services;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class HeroManager : MonoBehaviour, IHeroManager
    {
        private readonly Dictionary<int, int> levelExperienceMap = new Dictionary<int, int>();
        
        [SerializeField] private AnimationCurve experienceCurve;
        [SerializeField] private List<AbilityBaseInfo> heroAbilities = new List<AbilityBaseInfo>();
        
        private ISelectorController selectorController;
        private ICameraController cameraController;
        private IBuildingManager buildingManager;
        private IInputManager inputManager;

        private bool heroIsRespawning = false;
       
        private int heroExperience = 0;
        private int heroLvl = 0;
        private int heroSkillPoints = 1;
        
        private Hero hero;
        private AbilityController abilityController;
        
        private EventTrigger eventTrigger;
        
        public Hero GetHero()
        {
            return hero;
        }

        public void OnHeroDie(Hero hero)
        {
            selectorController.DeselectObject(hero);
            cameraController.SetFreeCameraMode();
            heroIsRespawning = true;
            SendHeroChangeStateEvent();
        }

        public Vector3 GetHeroPosition()
        {
            return heroIsRespawning ? buildingManager.GetSanctumPosition() : hero.transform.position;
        }

        public bool HeroIsRespawning()
        {
            return heroIsRespawning;
        }

        public void OnHeroRespawn(GameUnit gameUnit)
        {
            hero = gameUnit as Hero;
            hero.SetLevel(heroLvl);
            heroIsRespawning = false;
        }

        public void AddExperience(int experience)
        {
            heroExperience += experience;
            int nextLevelExperience = levelExperienceMap[heroLvl + 1];
            
            if (heroExperience >= nextLevelExperience)
            {
                heroLvl++;
                heroSkillPoints++;
                hero.SetLevel(heroLvl);
            }
            
            SendHeroChangeStateEvent();
        }
        
        public void OnHPChange()
        {
            SendHeroChangeStateEvent();
        }

        public int GetHeroLevel()
        {
            return heroLvl;
        }

        public void OnAbilityUpgradeClick(AbilityType abilityType)
        {
            if (heroSkillPoints <= 0)
            {
                return;
            }

            abilityController.UpgradeAbility(abilityType);
            heroSkillPoints--;
            SendHeroChangeStateEvent();
        }

        public bool IsCastingSpell()
        {
            return abilityController.IsCastingSpell();
        }

        public void AddListener(Action<HeroChangeStateEventData> listener)
        {
            eventTrigger.AddListener<HeroChangeState, HeroChangeStateEventData>(listener);
        }

        public void RemoveListener(Action<HeroChangeStateEventData> listener)
        {
            eventTrigger.RemoveListener<HeroChangeState, HeroChangeStateEventData>(listener);
        }
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IHeroManager>(this);
            
            eventTrigger = new EventTrigger();
            ConvertCurveToLevelExperienceThresholds();
        }
        
        private void Start()
        {
            selectorController = ServiceLocator.Instance.Get<ISelectorController>();
            cameraController = ServiceLocator.Instance.Get<ICameraController>();
            buildingManager = ServiceLocator.Instance.Get<IBuildingManager>();
            inputManager = ServiceLocator.Instance.Get<IInputManager>();
            
            
            InstantHeroSpawn();
            abilityController = new AbilityController(heroAbilities);
        }
        
        private void InstantHeroSpawn()
        {
            buildingManager.GetSanctum().InstantHeroSpawn(); 
        }
        
        private void ConvertCurveToLevelExperienceThresholds()
        {
            if (experienceCurve.length == 0)
            {
                return;
            }

            int maxLevel = Mathf.RoundToInt(experienceCurve.keys[experienceCurve.length - 1].time);
            
            for (int i = 0; i <= maxLevel; i++)
            {
                levelExperienceMap[i] = (int) experienceCurve.Evaluate(i);
            }
            
            levelExperienceMap[maxLevel + 1] = int.MaxValue;
        }
        
        private void SendHeroChangeStateEvent()
        {
            float currentLevelExperience = levelExperienceMap[heroLvl];
            float nextLevelExperience = levelExperienceMap[heroLvl + 1];
            
            HeroChangeStateEventData eventData = new HeroChangeStateEventData
            {
                nextLevelRatio = (heroExperience - currentLevelExperience) / (nextLevelExperience - currentLevelExperience),
                skillPoints = heroSkillPoints,
                hpRatio = heroIsRespawning ? 0f : hero.GetHealthRatio(),
                heroLvl = heroLvl
            };
            eventTrigger.RaiseEvent<HeroChangeState, HeroChangeStateEventData>(eventData);
        }

        private void OnDestroy()
        {
            abilityController.OnDestroy();
        }
    }
}
