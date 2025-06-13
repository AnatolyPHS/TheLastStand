using GameSceneObjects.Units;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public interface IHeroManager
    {
        Hero GetHero();
        void OnHeroDie(Hero hero);
        Vector3 GetHeroPosition();
        bool HeroIsRespawning();
        void OnHeroRespawn(Unit unit);
    }
}