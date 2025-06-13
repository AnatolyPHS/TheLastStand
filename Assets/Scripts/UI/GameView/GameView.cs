using System.Collections.Generic;
using UnityEngine;

namespace UI.GameView
{
    public class GameView : MonoBehaviour, IGameView
    {
        [SerializeField] private List<View> views =  new List<View>();
        
        private void Awake()
        {
            foreach (var view in views)
            {
                view.Init();
            }
        }
    }
}
