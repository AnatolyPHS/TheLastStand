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
            Time.timeScale = 1f; //TODO: need time manager with pause and resume methods
        }
        
        private void Start()
        {
            endGameView = ServiceLocator.Instance.Get<IEndGameView>();
        }

        public void ShowEndGameView(string congratulationsYouHaveCompletedAllWaves)
        {
            Time.timeScale = 0f; //TODO: need time manager with pause and resume methods
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
