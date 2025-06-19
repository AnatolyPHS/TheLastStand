using System.Collections.Generic;
using EffectsManager;
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
        private IEffectHolder effectHolder;
        private IHeroManager heroManager;
        
        private AbilityType currentAbilityType = AbilityType.None;

        private InputAction inputLeftMouseClickAction;
        private InputAction mousePositionAction;
        
        public AbilityController(List<AbilityBaseInfo> heroAbilities)
        {
            this.inputManager = ServiceLocator.Instance.Get<IInputManager>();
            this.selectorController = ServiceLocator.Instance.Get<ISelectorController>();
            this.effectHolder = ServiceLocator.Instance.Get<IEffectHolder>();
            this.heroManager = ServiceLocator.Instance.Get<IHeroManager>();
            
            EquipAbilities(heroAbilities);
            
            inputLeftMouseClickAction = inputManager.GetInputAction(InputManager.LeftMouseClickActionKey);
            mousePositionAction = inputManager.GetInputAction(InputManager.MouseScreenPosActionKey);
            
            inputLeftMouseClickAction.performed += OnLeftMouseClick;
            mousePositionAction.performed += OnMousePositionUpdate;
        }

        public void UpgradeAbility(AbilityType abilityType)
        {
            equippedAbilities[abilityType].UpgradeAbility();
        }

        public bool IsCastingSpell()
        {
            foreach (KeyValuePair<AbilityType, AbilityBase> pair in equippedAbilities)
            {
                if (pair.Value.AbilityInUse)
                {
                    return true;
                }
            }
        
            return false;
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
        
        private void OnMousePositionUpdate(InputAction.CallbackContext obj)
        {
            if (currentAbilityType == AbilityType.None)
            {
                return;
            }
            
            Vector3 mouseGroundPosition = selectorController.RecalculateWorldPointUnderMouse(mousePositionAction.ReadValue<Vector2>());
            equippedAbilities[currentAbilityType].OnUpdate(mouseGroundPosition);
        }

        private void OnLeftMouseClick(InputAction.CallbackContext obj)
        {
            if (currentAbilityType == AbilityType.None){
                return;
            }
            
            Vector3 mouseGroundPosition = selectorController.RecalculateWorldPointUnderMouse(mousePositionAction.ReadValue<Vector2>());
            equippedAbilities[currentAbilityType].ActivateAbility(mouseGroundPosition);
        }
        
        internal void SetCurrentAbilityType(AbilityType abilityType)
        {
            currentAbilityType = abilityType;
        }
        
        private void EquipAbilities(List<AbilityBaseInfo> heroAbilities)
        {
            this.heroAbilities = heroAbilities;
            
            for (int i = 0; i < heroAbilities.Count; i++) 
            {
                switch (heroAbilities[i].AbilityType)
                {
                    case AbilityType.MeteorShower:
                        equippedAbilities[AbilityType.MeteorShower] = new MeteorShowerAbility(heroAbilities[i], heroManager, effectHolder, this);
                        inputManager.SubscribeToInputEvent(heroAbilities[i].AbilityInputType, equippedAbilities[AbilityType.MeteorShower].AbilityButtonClick);
                        break;
                    case AbilityType.FreezingArrow:
                        equippedAbilities[AbilityType.FreezingArrow] = new FreezingArrowAbility(heroAbilities[i], effectHolder, heroManager, this);
                        inputManager.SubscribeToInputEvent(heroAbilities[i].AbilityInputType, equippedAbilities[AbilityType.FreezingArrow].AbilityButtonClick);
                        break;
                    default:
                        throw new System.NotImplementedException($"Ability type {heroAbilities[i].AbilityType} is not implemented.");
                }
            }
        }
    }
}
