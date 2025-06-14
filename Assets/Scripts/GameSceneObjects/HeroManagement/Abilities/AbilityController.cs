using System.Collections.Generic;
using GameSceneObjects.Data.AbilityData;
using InputsManager;
using Selector;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameSceneObjects.HeroManagement.Abilities
{
    public class AbilityController 
    {
        private readonly Dictionary<AbilityType, AbilityBase> equippedAbilities = new Dictionary<AbilityType, AbilityBase>();

        private List<AbilityBaseInfo> heroAbilities;
        
        private IInputManager inputManager;
        private ISelectorController selectorController;
        
        private AbilityType currentAbilityType = AbilityType.None;
        
        public AbilityController(List<AbilityBaseInfo> heroAbilities)
        {
            this.inputManager = ServiceLocator.Instance.Get<IInputManager>();
            this.selectorController = ServiceLocator.Instance.Get<ISelectorController>();
            
            EquipAbilities(heroAbilities);
            
            InputAction inputLeftMouseClickAction = inputManager.GetInputAction(InputManager.LeftMouseClickActionKey);
            InputAction mousePositionAction = inputManager.GetInputAction(InputManager.MouseScreenPosActionKey);
            
            inputLeftMouseClickAction.performed += OnLeftMouseClick;
            mousePositionAction.performed += OnMousePositionUpdate;
        }

        private void OnMousePositionUpdate(InputAction.CallbackContext obj)
        {
            if (currentAbilityType == AbilityType.None)
            {
                return;
            }
            
            Vector3 mouseGroundPosition = selectorController.GetCurrentWorldPoint();
            equippedAbilities[currentAbilityType].OnUpdate(mouseGroundPosition);
        }

        private void OnLeftMouseClick(InputAction.CallbackContext obj)
        {
            if (currentAbilityType == AbilityType.None){
                return;
            }
            
            Vector3 mouseGroundPosition = selectorController.GetCurrentWorldPoint();
            equippedAbilities[currentAbilityType].ActivateAbility(mouseGroundPosition);
        }

        public bool CanUseAbility()
        {
            return currentAbilityType == AbilityType.None;
        }

        internal void SetCurrentAbilityType(AbilityType abilityType)
        {
            currentAbilityType = abilityType;
        }
        
        public void OnDestroy()
        {
            for (int i = 0; i < heroAbilities.Count; i++) 
            {
                if (equippedAbilities.ContainsKey(heroAbilities[i].AbilityType))
                {
                    inputManager.UnsubscribeFromInputEvent(heroAbilities[i].AbilityInputType, equippedAbilities[heroAbilities[i].AbilityType].AbilityButtonClick);
                }
            }
        }
        
        private void EquipAbilities(List<AbilityBaseInfo> heroAbilities)
        {
            this.heroAbilities = heroAbilities;
            
            for (int i = 0; i < heroAbilities.Count; i++) 
            {
                switch (heroAbilities[i].AbilityType)
                {
                    case AbilityType.MeteorShower:
                        equippedAbilities[AbilityType.MeteorShower] = new MeteorShowerAbility(heroAbilities[i], this);
                        inputManager.SubscribeToInputEvent(heroAbilities[i].AbilityInputType, equippedAbilities[AbilityType.MeteorShower].AbilityButtonClick);
                        break;
                    case AbilityType.FreezingArrow:
                        equippedAbilities[AbilityType.FreezingArrow] = new FreezingArrowAbility(heroAbilities[i], this);
                        inputManager.SubscribeToInputEvent(heroAbilities[i].AbilityInputType, equippedAbilities[AbilityType.FreezingArrow].AbilityButtonClick);
                        break;
                    default:
                        throw new System.NotImplementedException($"Ability type {heroAbilities[i].AbilityType} is not implemented.");
                }
            }
        }
    }
}
