using GameSceneObjects.Units;
using Services;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class HeroManager : MonoBehaviour, IHeroManager
    {
        [SerializeField] private Hero hero;

        public Hero GetHero()
        {
            return hero;
        }

        private void Awake()
        {
            ServiceLocator.Instance.Register<IHeroManager>(this);
        }
    }

    public interface IHeroManager
    {
        Hero GetHero();
    }
}
