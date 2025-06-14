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
        
        public override void Init()
        {
            ServiceLocator.Instance.Register<IHeroView>(this);
        }

        public override void OnMainUIStart()
        {
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
        }

        public void UpdateHP(float hpRatio)
        {
            hpProgressBar.fillAmount = hpRatio;
        }
    }
}
