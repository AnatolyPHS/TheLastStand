using System;
using InputsManager;
using Services;
using UnityEngine;

namespace UI.GameView
{
    public class BuiildingView : View, IBuildingView
    {
        [SerializeField] private GameObject buildingPanel;
        
        private IInputManager inputManager;
        
        public override void Init()
        {
            ServiceLocator.Instance.Register<IBuildingView>(this);
        }
        
        private void Start()
        {
            inputManager = ServiceLocator.Instance.Get<IInputManager>();
            
            inputManager.SubscribeToInputEvent(InputType.AllyBuildingSelected, OnAllySpawnerSelected);
        }

        private void OnAllySpawnerSelected(float value)
        {
            buildingPanel.SetActive(value > float.Epsilon);
        }
    }
}
