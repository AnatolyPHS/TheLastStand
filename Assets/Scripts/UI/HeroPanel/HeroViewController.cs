using GameSceneObjects.HeroManagement;
using Services;
using UnityEngine;

namespace UI.HeroPanel
{
    public class HeroViewController : MonoBehaviour, IHeroViewController
    {
        private IHeroManager heroManager;
        private IHeroView heroView;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IHeroViewController>(this);
        }

        private void Start()
        {
            heroManager = ServiceLocator.Instance.Get<IHeroManager>();
            heroView = ServiceLocator.Instance.Get<IHeroView>();
            
            heroManager.AddListener(OnHeroStateChanged);
        }

        private void OnHeroStateChanged(HeroChangeStateEventData obj)
        {
            heroView.UpdateHeroExperience(obj.nextLevelRatio);
            heroView.UpdateHeroSkillPoints(obj.skillPoints);
            heroView.UpdateHeroLevel(obj.heroLvl);
            heroView.UpdateHP(obj.hpRatio);
        }

        public void OnAbilityUpgradeClick(AbilityType abilityType)
        {
            heroManager.OnAbilityUpgradeClick(abilityType);
        }
    }
}
