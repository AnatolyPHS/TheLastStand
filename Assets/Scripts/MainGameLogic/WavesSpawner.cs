using GameSceneObjects.Buildings;
using GameSceneObjects.Units;
using Services;
using UI.GameView;
using UnityEngine;

namespace MainGameLogic
{
    public class WavesSpawner : MonoBehaviour, IWavesSpawner
    {
        [SerializeField] private float firstWaveDelay = 10f;
        [SerializeField] private float nextWaveDelay = 5f;
        [SerializeField] private int numberOfWaves = 6;

        private IBuildingManager buildingManager;
        private IUnitHolder unitHolder;
        private IEndGameViewController endGameViewController;
        
        private int currentWave = 0;

        private float nextWaveTime = float.MaxValue;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IWavesSpawner>(this);
        }
        
        private void Start()
        {
            buildingManager = ServiceLocator.Instance.Get<IBuildingManager>();
            unitHolder = ServiceLocator.Instance.Get<IUnitHolder>();
            endGameViewController = ServiceLocator.Instance.Get<IEndGameViewController>();
            
            //TODO: run UI timer if any
            nextWaveTime = Time.time + firstWaveDelay;
        }

        private void Update() //TODO: add spawning states
        {
            if (WaveInProgress())
            {
                return;
            }
            
            if (Time.time < nextWaveTime)
            {
                return;
            }
            
            if (currentWave < numberOfWaves)
            {
                currentWave++;
                SpawnNextWave();
            }
            else
            {
                endGameViewController.ShowEndGameView("Congratulations! You have completed all waves!");
            }
        }

        private bool WaveInProgress()
        {
            return buildingManager.EnemiesAreSpawning() || unitHolder.HasEnemiesOnField();
        }

        private void SpawnNextWave()
        {
            buildingManager.SetEnemySpawnersLevel(currentWave);
        }
    }
}
