using TMPro;
using UnityEngine;

namespace MenuScripts
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private string gameSceneName = "GameScene";
        [SerializeField] private GameObject buttonsHolder;
        [SerializeField] private TextMeshProUGUI loadingText;
        
        public void OnPlayButtonClicked()
        {
            buttonsHolder.SetActive(false);
            loadingText.gameObject.SetActive(true);
            loadingText.text = "Loading...";
            
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameSceneName).completed += OnSceneLoaded;
        }
        
        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }

        public void OnSettingsButtonClicked()
        {
            //TODO: adds sound and quality settings
        }
        
        private void OnSceneLoaded(AsyncOperation obj)
        {
            //TODO: loading bar
        }

        private void Start()
        {
            buttonsHolder.SetActive(true);
            loadingText.gameObject.SetActive(false);
        }
    }
}
