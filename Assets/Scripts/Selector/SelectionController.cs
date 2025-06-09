using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Selector
{
    public class SelectionController : MonoBehaviour
    {
        private const float YBoundSelectionThreshold = 100f;

        [SerializeField][Header("Input Actions")]
        private InputActionAsset playerInputActions; //TODO: use service locator

        [SerializeField][Header("UI Reference")]
        private RectTransform selectionRectangleUI;

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
            selectionRectangleUI.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            if (isDragging == false)
            {
                return;
            }
            
            currentScreenMousePosition = mousePositionAction.ReadValue<Vector2>();
            UpdateWorldAndScreenPositions();
            UpdateSelectionRectangleUI();
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
            selectionRectangleUI.gameObject.SetActive(false);

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


        private void UpdateSelectionRectangleUI() //TODO: move to the view
        {
            Vector2 projectedStartScreen = mainCamera.WorldToScreenPoint(startWorldPoint);
            Vector2 projectedCurrentScreen = mainCamera.WorldToScreenPoint(currentWorldPoint);
            
            float x1 = Mathf.Min(projectedStartScreen.x, projectedCurrentScreen.x);
            float y1 = Mathf.Min(projectedStartScreen.y, projectedCurrentScreen.y);
            float x2 = Mathf.Max(projectedStartScreen.x, projectedCurrentScreen.x);
            float y2 = Mathf.Max(projectedStartScreen.y, projectedCurrentScreen.y);

            Rect screenRect = new Rect(x1, y1, x2 - x1, y2 - y1);
            
            if (screenRect.width < minDragDistance && screenRect.height < minDragDistance)
            {
                selectionRectangleUI.gameObject.SetActive(false);
                return;
            }

            if (selectionRectangleUI.gameObject.activeSelf == false)
            {
                selectionRectangleUI.gameObject.SetActive(true);
            }
            
            Vector2 rectCenter = screenRect.center;
            Vector2 canvasCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

            selectionRectangleUI.anchoredPosition = rectCenter - canvasCenter;
            selectionRectangleUI.sizeDelta = screenRect.size;
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