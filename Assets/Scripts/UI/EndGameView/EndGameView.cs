using Services;
using TMPro;
using UI.GameView;
using UnityEngine;

namespace UI.EndGameView
{
    public class EndGameView : View, IEndGameView
    {
        [SerializeField] private TextMeshProUGUI endGameText;
        [SerializeField] private GameObject endGamePanel;
        
        private IEndGameViewController endGameViewController;
        
        public void ShowEndGamePanel(string message)
        {
            endGameText.text = message;
            endGamePanel.SetActive(true);
        }
        
        public override void Init()
        {
            ServiceLocator.Instance.Register<IEndGameView>(this);
        }

        public override void OnMainUIStart()
        {
            endGameViewController = ServiceLocator.Instance.Get<IEndGameViewController>();
        }

        public void OnRestartClicked()
        {
            endGameViewController.OnRestartClicked();
        }

        public void OnQuitClicked()
        {
            endGameViewController.OnQuitClicked();
        }
    }
}
