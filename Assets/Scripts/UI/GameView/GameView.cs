using Services;
using UnityEngine;

namespace UI.GameView
{
    public class GameView : MonoBehaviour, IGameView
    {
        [SerializeField] private GameObject buildingPanel;
        
        public void SetBuildingPanelState(bool state)
        {
            buildingPanel.SetActive(state);
        }
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<IGameView>(this);
        }
    }
}
