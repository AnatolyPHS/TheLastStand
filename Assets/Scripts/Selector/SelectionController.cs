using System.Collections.Generic;
using Camera;
using GameSceneObjects;
using GameSceneObjects.Buildings;
using GameSceneObjects.Units;
using InputsManager;
using Services;
using UI.SelectorView;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Selector
{
    public class SelectionController : MonoBehaviour, ISelectorController
    {
        private const float YBoundSelectionThreshold = 20f;
        
        private readonly List<IClickSelectable> currentlySelectedObjects = new List<IClickSelectable>();
        private readonly List<IClickSelectable> selectedObjectToPrioritise = new List<IClickSelectable>();
        
        [SerializeField][Header("Selection Settings")]
        private LayerMask selectableLayer;
        [SerializeField] private float minDragDistance = 5f;
        [SerializeField] private float groundPlaneHeight = 0f;
        
        private IInputManager inputManager;
        private ISelectorView selectorView;
        
        private Vector3 startWorldPoint; 
        private Vector3 currentWorldPoint;

        private Vector2 startScreenMousePosition; 
        private Vector2 currentScreenMousePosition;

        private InputAction leftClickAction;
        private InputAction mousePositionAction;
        private InputAction rightmouseClickAction;

        private bool isDragging = false;
        
        private Plane groundPlane;
        private UnityEngine.Camera mainCamera;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<ISelectorController>(this);
        }

        private void Start()
        {
            inputManager = ServiceLocator.Instance.Get<IInputManager>();
            selectorView = ServiceLocator.Instance.Get<ISelectorView>();
            ICameraController cameraController = ServiceLocator.Instance.Get<ICameraController>();
            
            mainCamera = cameraController.GetCamera();

            leftClickAction = inputManager.GetInputAction(InputManager.LeftMouseClickActionKey);
            mousePositionAction = inputManager.GetInputAction(InputManager.MouseScreenPosActionKey);
            rightmouseClickAction = inputManager.GetInputAction(InputManager.RightMouseClickActionKey);
            
            leftClickAction.started += OnLeftClickStarted;
            leftClickAction.canceled += OnLeftClickCanceled;
            
            rightmouseClickAction.performed += OnRightClickPerformed;
            
            groundPlane = new Plane(Vector3.up, Vector3.up * groundPlaneHeight);
            
            leftClickAction.Enable();
            mousePositionAction.Enable();
            rightmouseClickAction.Enable();
            
            selectorView.SetSelectorState(false);
        }

        private void OnRightClickPerformed(InputAction.CallbackContext obj)
        {
            if (isDragging)
            {
                return;
            }

            currentScreenMousePosition = mousePositionAction.ReadValue<Vector2>();
            currentWorldPoint = RecalculateWorldPointUnderMouse(currentScreenMousePosition);

            Ray ray = mainCamera.ScreenPointToRay(currentScreenMousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayer)//TODO: calculate max cast limit
                && hit.collider.TryGetComponent(out Unit unit))
            {
                CommandSelectedToInteract(unit);
            }
            else
            {
                CommandSelectedToMove(currentWorldPoint);
            }
        }

        private void CommandSelectedToInteract(Unit unt)
        {
            for (int i = 0; i < currentlySelectedObjects.Count; i++)
            {
                if (currentlySelectedObjects[i] is IClickInteractable interactable)
                {
                    interactable.InteractWithUnit(unt);
                }
            }
        }
        
        private void CommandSelectedToMove(Vector3 targetPosition)
        {
            for (int i = 0; i < currentlySelectedObjects.Count; i++)
            {
                if (currentlySelectedObjects[i] is IClickInteractable interactable)
                {
                    interactable.MoveTo(targetPosition);
                }
            }
        }

        private void Update()
        {
            if (isDragging == false)
            {
                return;
            }
            
            currentScreenMousePosition = mousePositionAction.ReadValue<Vector2>();
            UpdateWorldAndScreenPositions();
            selectorView.UpdateSelector(startWorldPoint, currentWorldPoint, minDragDistance);
        }

        private void OnLeftClickStarted(InputAction.CallbackContext context)
        {
            if (inputManager.IsPointerOverGameObject())
            {
                return;
            }
            
            startScreenMousePosition = mousePositionAction.ReadValue<Vector2>();

            startWorldPoint = RecalculateWorldPointUnderMouse(startScreenMousePosition);
            isDragging = true; 
            ClearSelection();
        }

        private void OnLeftClickCanceled(InputAction.CallbackContext context)
        {
            if (isDragging == false)
            {
                return;
            }

            isDragging = false;
            selectorView.SetSelectorState(false);

            currentScreenMousePosition = mousePositionAction.ReadValue<Vector2>();
            
            float screenDragDistance = Vector2.Distance(startScreenMousePosition, currentScreenMousePosition);

            if (screenDragDistance < minDragDistance)
            {
                HandleSingleClickSelection();
            }
            else
            {
                currentWorldPoint = RecalculateWorldPointUnderMouse(currentScreenMousePosition);
                SelectUnitsInRectangle(startWorldPoint, currentWorldPoint);
            }
        }

        private void UpdateWorldAndScreenPositions()
        {
            currentWorldPoint = RecalculateWorldPointUnderMouse(currentScreenMousePosition);
        }
        
        private Vector3 RecalculateWorldPointUnderMouse(Vector2 screenPosition)
        {
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            
            float distance;
            if (groundPlane.Raycast(ray, out distance))
            {
                return ray.GetPoint(distance);
            }
            
            return Vector3.zero; 
        }

        private void SelectUnitsInRectangle(Vector3 startWorldPoint, Vector3 endWorldPoint)
        {
            Bounds selectionBounds = new Bounds();
            selectionBounds.SetMinMax(
                new Vector3(Mathf.Min(startWorldPoint.x, endWorldPoint.x), -YBoundSelectionThreshold, Mathf.Min(startWorldPoint.z, endWorldPoint.z)),
                new Vector3(Mathf.Max(startWorldPoint.x, endWorldPoint.x), YBoundSelectionThreshold, Mathf.Max(startWorldPoint.z, endWorldPoint.z))
            );

            Collider[] hits = Physics.OverlapBox(selectionBounds.center, selectionBounds.extents, Quaternion.identity, selectableLayer);

            selectedObjectToPrioritise.Clear();
            
            foreach (Collider col in hits)
            {
                if (col.TryGetComponent(out IClickSelectable selectable) && selectedObjectToPrioritise.Contains(selectable) == false)
                {
                    selectedObjectToPrioritise.Add(selectable);
                }
            }
            
            PrioritiseSelectedObjects();
        }

        private void PrioritiseSelectedObjects()
        {
            IClickSelectable buildingToSelect = null;
            for (int i = 0; i < selectedObjectToPrioritise.Count; i++)
            {
                IClickSelectable selectable = selectedObjectToPrioritise[i];
                if (selectable is Unit)
                {
                    SelectObject(selectable);
                }

                if (selectable is BuildingBase)
                {
                    buildingToSelect = selectable;
                }
            }

            if (currentlySelectedObjects.Count == 0 && buildingToSelect != null)
            {
                SelectObject(buildingToSelect);
            }
        }

        private void HandleSingleClickSelection()
        {
            Ray ray = mainCamera.ScreenPointToRay(currentScreenMousePosition);
            RaycastHit hit;

             if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayer)
                && hit.collider.TryGetComponent(out IClickSelectable selectable))
            {
                ClearSelection();
                SelectObject(selectable);
            }
        }

        private void SelectObject(IClickSelectable selectable)
        {
            if (!currentlySelectedObjects.Contains(selectable))
            {
                selectable.OnSelect();
                currentlySelectedObjects.Add(selectable);
            }
        }

        public void DeselectObject(IClickSelectable selectable)
        {
            IClickInteractable interactable = selectable as IClickInteractable;
            if (currentlySelectedObjects.Contains(interactable))
            {
                interactable.OnDeselect();
                currentlySelectedObjects.Remove(interactable);
            }
        }
        
        private void ClearSelection()
        {
            foreach (IClickSelectable selectable in currentlySelectedObjects)
            {
                selectable.OnDeselect();
            }
            currentlySelectedObjects.Clear();
        }

        private void OnDestroy()
        {
            leftClickAction.started -= OnLeftClickStarted;
            leftClickAction.canceled -= OnLeftClickCanceled;
            rightmouseClickAction.performed -= OnRightClickPerformed;

            leftClickAction.Disable();
            mousePositionAction.Disable();
            rightmouseClickAction.Disable();
            
            ServiceLocator.Instance.Unregister<ISelectorController>();
        }
    }
}