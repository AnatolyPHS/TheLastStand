using System.Collections.Generic;
using GameSceneObjects.HeroManagement;
using Services;
using TMPro;
using UI.GameView;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HeroPanel
{
    public class HeroView : View, IHeroView
    {
        [SerializeField] private Image expProgressBar;
        [SerializeField] private Image hpProgressBar;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI skillPointsText;
        [SerializeField] private List<Button> upgradeButtons = new List<Button>(); //TODO: add ability cooldown UIs
        
        private IHeroViewController heroViewController;
        
        public override void Init()
        {
            ServiceLocator.Instance.Register<IHeroView>(this);
        }

        public override void OnMainUIStart()
        {
            heroViewController = ServiceLocator.Instance.Get<IHeroViewController>();
            gameObject.SetActive(true);
        }

        public void UpdateHeroLevel(int lvl)
        {
            levelText.text = lvl.ToString();
        }

        public void UpdateHeroExperience(float expRatio)
        {
            expProgressBar.fillAmount = expRatio;
        }

        public void UpdateHeroSkillPoints(float skillPoints)
        {
            skillPointsText.text = skillPoints.ToString();
            for (int i = 0; i < upgradeButtons.Count; i++)
            {
                upgradeButtons[i].gameObject.SetActive(skillPoints > 0);
            }
        }

        public void UpdateHP(float hpRatio)
        {
            hpProgressBar.fillAmount = hpRatio;
        }
        
        public void OnAbilityUpgradeClick(int ability)
        {
            AbilityType abilityType = (AbilityType)ability;
            heroViewController.OnAbilityUpgradeClick(abilityType);
        }
    }
}
