using GameSceneObjects.Buildings;
using GameSceneObjects.Units;
using Services;
using UI.GameView;
using UI.WavesPanel;
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
        private IGameProgressViewController gameProgressViewController;
        
        private int currentWave = 0;

        private float nextWaveTimer = float.MaxValue;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IWavesSpawner>(this);
        }
        
        private void Start()
        {
            buildingManager = ServiceLocator.Instance.Get<IBuildingManager>();
            unitHolder = ServiceLocator.Instance.Get<IUnitHolder>();
            endGameViewController = ServiceLocator.Instance.Get<IEndGameViewController>();
            gameProgressViewController = ServiceLocator.Instance.Get<IGameProgressViewController>();
            
            //TODO: run UI timer if any
            nextWaveTimer = firstWaveDelay;
            
            gameProgressViewController.UpdateWaveProgress(numberOfWaves - currentWave);
        }

        private void Update() //TODO: add spawning states
        {
            if (WaveInProgress())
            {
                return;
            }
            
            if (nextWaveTimer > 0f)
            {
                nextWaveTimer -= Time.deltaTime;
                return;
            }
            
            if (currentWave < numberOfWaves)
            {
                nextWaveTimer = nextWaveDelay;
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
            gameProgressViewController.UpdateWaveProgress(numberOfWaves - currentWave);
            buildingManager.SetEnemySpawnersLevel(currentWave);
        }
    }
}
