using System.Collections.Generic;
using UnityEngine;

namespace UI.GameView
{
    public class GameView : MonoBehaviour, IGameView
    {
        [SerializeField] private List<View> views =  new List<View>();
        
        public void QuitGame() //TODO: replace with open menu button and add menu view
        {
            Application.Quit();
        }
        
        private void Awake()
        {
            foreach (View view in views)
            {
                view.Init();
            }
        }
        
        private void Start()
        {
            foreach (View view in views)
            {
                view.OnMainUIStart();
            }
        }
    }
}
