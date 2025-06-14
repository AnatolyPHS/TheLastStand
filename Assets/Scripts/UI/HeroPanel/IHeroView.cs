namespace UI.HeroPanel
{
    public interface IHeroView
    {
        void UpdateHeroLevel(int lvl);
        void UpdateHeroExperience(float expRatio);
        void UpdateHeroSkillPoints(float skillPoints);
        void UpdateHP(float hpRatio);
    }
}
