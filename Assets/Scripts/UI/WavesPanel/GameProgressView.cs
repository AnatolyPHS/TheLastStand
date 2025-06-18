using Services;
using TMPro;
using UI.GameView;
using UnityEngine;

namespace UI.WavesPanel
{
    public class GameProgressView : View, IGameProgressView
    {
        [SerializeField] private TextMeshProUGUI wavesLeftText;
        
        public override void Init()
        {
            ServiceLocator.Instance.Register<IGameProgressView>(this);
        }

        public void UpdateWavesLeftText(int wavesLeft)
        {
            wavesLeftText.text = wavesLeft > 0  ? $"Waves left: {wavesLeft}" : "The last stand!";
        }
    }
}
