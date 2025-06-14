using System;
using System.Collections.Generic;
using Camera;
using GameEvents;
using GameSceneObjects.Buildings;
using GameSceneObjects.Units;
using Selector;
using Services;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class HeroManager : MonoBehaviour, IHeroManager
    {
        private readonly Dictionary<int, float> levelExperienceMap = new Dictionary<int, float>();
        
        [SerializeField] private AnimationCurve experienceCurve;
        
        private ISelectorController selectorController;
        private ICameraController cameraController;
        private IBuildingManager buildingManager;

        private bool heroIsRespawning = false;
       
        private float heroExperience = 0f;
        private int heroLvl = 0;
        private float heroSkillPoints = 0f;
        
        private Hero hero;
        
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

        public void OnHeroRespawn(Unit unit)
        {
            hero = unit as Hero;
            heroIsRespawning = false;
        }

        public void AddExperience(int experience)
        {
            heroExperience += experience;
            float nextLevelExperience = levelExperienceMap[heroLvl + 1];
            
            if (heroExperience >= nextLevelExperience)
            {
                heroExperience -= nextLevelExperience;
                heroLvl++;
                heroSkillPoints++;
            }
            
            SendHeroChangeStateEvent();
        }
        
        public void OnHPChange()
        {
            SendHeroChangeStateEvent();
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
            
            InstantHeroSpawn();
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
                levelExperienceMap[i] = experienceCurve.Evaluate(i);
            }
            
            levelExperienceMap[maxLevel + 1] = float.MaxValue;
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
    }
}
