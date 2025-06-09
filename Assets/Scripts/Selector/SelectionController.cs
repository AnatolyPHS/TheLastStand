using System.Collections.Generic;
using UI.SelectorView;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Selector
{
    public class SelectionController : MonoBehaviour
    {
        private const float YBoundSelectionThreshold = 100f;

        [SerializeField][Header("Input Actions")]
        private InputActionAsset playerInputActions; //TODO: use service locator

        [SerializeField] [Header("UI view Reference")]
        /*private RectTransform selectionRectangleUI;*/
        private SelectorView selectorView;

        [SerializeField][Header("Selection Settings")]
        private LayerMask selectableLayer;
        [SerializeField] private UnityEngine.Camera mainCamera; //TODO: use servicde locator and take it from CameraMover
        [SerializeField] private float minDragDistance = 5f;
        [SerializeField] private float groundPlaneHeight = 0f;
        
        private Vector3 startWorldPoint; 
        private Vector3 currentWorldPoint;

        private Vector2 startScreenMousePosition; 
        private Vector2 currentScreenMousePosition;

        private InputAction leftClickAction;
        private InputAction mousePositionAction;

        private bool isDragging = false;

        private List<ISelectable> currentlySelectedObjects = new List<ISelectable>();
        
        Plane groundPlane;

        private void Awake()
        {
            InputActionMap gameplayActionMap = playerInputActions.FindActionMap("Gameplay"); //TODO: additional manager or at least a class with names

            leftClickAction = gameplayActionMap.FindAction("LeftClick");
            mousePositionAction = gameplayActionMap.FindAction("MouseScreenPos");

            leftClickAction.started += OnLeftClickStarted;
            leftClickAction.canceled += OnLeftClickCanceled;
            
            groundPlane = new Plane(Vector3.up, Vector3.up * groundPlaneHeight);
        }

        private void OnEnable()
        {
            leftClickAction.Enable();
            mousePositionAction.Enable();
            selectorView.SetSelectorState(false);
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

            foreach (Collider col in hits)
            {
                if (col.TryGetComponent(out ISelectable selectable))
                {
                    SelectObject(selectable);
                }
            }
        }

        private void HandleSingleClickSelection()
        {
            Ray ray = mainCamera.ScreenPointToRay(currentScreenMousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayer)
                && hit.collider.TryGetComponent(out ISelectable selectable))
            {
                ClearSelection();
                SelectObject(selectable);
            }
        }

        private void SelectObject(ISelectable selectable)
        {
            if (!currentlySelectedObjects.Contains(selectable))
            {
                selectable.OnSelect();
                currentlySelectedObjects.Add(selectable);
            }
        }

        private void ClearSelection()
        {
            foreach (ISelectable selectable in currentlySelectedObjects)
            {
                selectable.OnDeselect();
            }
            currentlySelectedObjects.Clear();
        }
    }
}