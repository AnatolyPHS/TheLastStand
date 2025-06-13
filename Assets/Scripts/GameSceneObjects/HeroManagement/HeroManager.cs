using Camera;
using GameSceneObjects.Buildings;
using GameSceneObjects.Units;
using Selector;
using Services;
using UnityEngine;

namespace GameSceneObjects.HeroManagement
{
    public class HeroManager : MonoBehaviour, IHeroManager
    {
        [SerializeField] private Transform heroGameStartPosition;
        
        private ISelectorController selectorController;
        private ICameraController cameraController;
        private IBuildingManager buildingManager;

        private bool heroIsRespawning = false;
        
        private Hero hero;
        
        public Hero GetHero()
        {
            return hero;
        }

        public void OnHeroDie(Hero hero)
        {
            selectorController.DeselectObject(hero);
            cameraController.SetFreeCameraMode();
            heroIsRespawning = true;
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

        private void Awake()
        {
            ServiceLocator.Instance.Register<IHeroManager>(this);
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
            buildingManager.GetSanctum().InstantHeroSpawn(heroGameStartPosition.position, heroGameStartPosition.rotation); 
        }
    }
}
