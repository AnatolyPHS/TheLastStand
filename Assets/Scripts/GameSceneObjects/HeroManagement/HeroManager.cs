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
        [SerializeField] private Hero hero;
        
        private ISelectorController selectorController;
        private ICameraController cameraController;
        private IBuildingManager buildingManager;

        private bool heroIsRespawning = false;
        
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

        private void Awake()
        {
            ServiceLocator.Instance.Register<IHeroManager>(this);
        }
        
        private void Start()
        {
            selectorController = ServiceLocator.Instance.Get<ISelectorController>();
            cameraController = ServiceLocator.Instance.Get<ICameraController>();
            buildingManager = ServiceLocator.Instance.Get<IBuildingManager>();
            
            hero.Init();//TODO: controll it on spawn
        }
    }
}
