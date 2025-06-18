using MainGameLogic;
using Services;
using UnityEngine;

namespace UI.WavesPanel
{
    public class GameProgressViewController : MonoBehaviour, IGameProgressViewController
    {
        private IGameProgressView gameProgressView;
        private IWavesSpawner wavesSpawner;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IGameProgressViewController>(this);
        }
        
        private void Start()
        {
            gameProgressView = ServiceLocator.Instance.Get<IGameProgressView>();
            wavesSpawner = ServiceLocator.Instance.Get<IWavesSpawner>();
        }

        public void UpdateWaveProgress(int wavesLeft)
        {
            gameProgressView.UpdateWavesLeftText(wavesLeft);
        }
    }
}
