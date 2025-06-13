using Services;
using UI.EndGameView;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.GameView
{
    public class EndGameViewController : MonoBehaviour, IEndGameViewController
    {
        private IEndGameView endGameView;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IEndGameViewController>(this);
        }
        
        private void Start()
        {
            endGameView = ServiceLocator.Instance.Get<IEndGameView>();
        }

        public void ShowEndGameView(string congratulationsYouHaveCompletedAllWaves)
        {
            endGameView.ShowEndGamePanel(congratulationsYouHaveCompletedAllWaves);
        }

        public void OnRestartClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}
