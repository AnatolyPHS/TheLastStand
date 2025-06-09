using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Needed for the Image component

namespace Selector
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField][Header("Input Actions")]
        private InputActionAsset playerInputActions; //TODO: use service locator

        [SerializeField][Header("UI Reference")]
        private RectTransform selectionRectangleUI;

        [SerializeField][Header("Selection Settings")]
        private LayerMask selectableLayer;
        [SerializeField] private UnityEngine.Camera mainCamera; //TODO: use servicde locator and take it from CameraMover
        [SerializeField] private float minDragDistance = 5f;

        [Header("Selection Visuals (UI Image)")]
        [SerializeField] private Color selectionFillColor = new Color(0.1f, 0.5f, 0.9f, 0.2f);
        
        private Vector3 startWorldPoint; 
        private Vector3 currentWorldPoint;

        private Vector2 startScreenMousePosition; 
        private Vector2 currentScreenMousePosition; // Current screen position of the mouse

        private InputAction leftClickAction;
        private InputAction mousePositionAction;

        private bool isDragging = false;

        private List<ISelectable> currentlySelectedObjects = new List<ISelectable>();

        private void Awake()
        {
            InputActionMap gameplayActionMap = playerInputActions.FindActionMap("Gameplay");

            leftClickAction = gameplayActionMap.FindAction("LeftClick");
            mousePositionAction = gameplayActionMap.FindAction("MouseScreenPos");

            leftClickAction.started += OnLeftClickStarted;
            leftClickAction.canceled += OnLeftClickCanceled;
        }

        private void OnEnable()
        {
            leftClickAction.Enable();
            mousePositionAction.Enable();
            selectionRectangleUI.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            if (isDragging)
            {
                currentScreenMousePosition = mousePositionAction.ReadValue<Vector2>();
                UpdateWorldAndScreenPositions();
                UpdateSelectionRectangleUI(); // Still use UI for drawing
            }
        }

        private void OnLeftClickStarted(InputAction.CallbackContext context)
        {
            startScreenMousePosition = mousePositionAction.ReadValue<Vector2>();
            
            if (TryGetWorldPointUnderMouse(startScreenMousePosition, out startWorldPoint))
            {
                isDragging = true;
                ClearSelection();
            }
        }

        private void OnLeftClickCanceled(InputAction.CallbackContext context)
        {
            if (isDragging == false)
            {
                return;
            }

            isDragging = false;
            selectionRectangleUI.gameObject.SetActive(false); // Hide UI rectangle

            currentScreenMousePosition = mousePositionAction.ReadValue<Vector2>(); // Final screen position

            // Determine if it was a short click or a drag based on screen distance
            float screenDragDistance = Vector2.Distance(startScreenMousePosition, currentScreenMousePosition);

            if (screenDragDistance < minDragDistance)
            {
                HandleSingleClickSelection();
            }
            else
            {
                // Ensure currentWorldPoint is up-to-date for selection logic
                TryGetWorldPointUnderMouse(currentScreenMousePosition, out currentWorldPoint);
                SelectUnitsInRectangle(startWorldPoint, currentWorldPoint);
            }
        }

        private void UpdateWorldAndScreenPositions()
        {
            // Continuously update the current world point for accurate selection
            TryGetWorldPointUnderMouse(currentScreenMousePosition, out currentWorldPoint);
        }


        private void UpdateSelectionRectangleUI()
        {
            // Project the startWorldPoint (fixed) and currentWorldPoint (dynamic) back to screen space
            // This ensures the screen rectangle always matches the world selection box
            Vector2 projectedStartScreen = mainCamera.WorldToScreenPoint(startWorldPoint);
            Vector2 projectedCurrentScreen = mainCamera.WorldToScreenPoint(currentWorldPoint);

            // Calculate the screen-space rectangle from the projected world points
            float x1 = Mathf.Min(projectedStartScreen.x, projectedCurrentScreen.x);
            float y1 = Mathf.Min(projectedStartScreen.y, projectedCurrentScreen.y);
            float x2 = Mathf.Max(projectedStartScreen.x, projectedCurrentScreen.x);
            float y2 = Mathf.Max(projectedStartScreen.y, projectedCurrentScreen.y);

            Rect screenRect = new Rect(x1, y1, x2 - x1, y2 - y1);

            // Only show the UI rectangle if the projected drag distance is sufficient
            // This prevents a tiny rectangle from appearing on a single click.
            if (screenRect.width < minDragDistance && screenRect.height < minDragDistance)
            {
                selectionRectangleUI.gameObject.SetActive(false);
                return;
            }

            if (!selectionRectangleUI.gameObject.activeSelf)
            {
                selectionRectangleUI.gameObject.SetActive(true);
                // Set initial color if not set in Inspector
                if (selectionRectangleUI.TryGetComponent(out Image img))
                {
                    img.color = selectionFillColor;
                }
            }

            // Position and size the RectTransform
            Vector2 rectCenter = screenRect.center;
            Vector2 canvasCenter = new Vector2(Screen.width / 2f, Screen.height / 2f); // Assuming full-screen overlay Canvas

            selectionRectangleUI.anchoredPosition = rectCenter - canvasCenter;
            selectionRectangleUI.sizeDelta = screenRect.size;
        }

        // Helper to get a world point on the ground plane under the mouse cursor
        private bool TryGetWorldPointUnderMouse(Vector2 screenPosition, out Vector3 worldPoint)
        {
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Assuming Y=0 is your ground plane

            float distance;
            if (groundPlane.Raycast(ray, out distance))
            {
                worldPoint = ray.GetPoint(distance);
                return true;
            }
            worldPoint = Vector3.zero; // Default if no hit
            return false;
        }

        private void SelectUnitsInRectangle(Vector3 startWorldPoint, Vector3 endWorldPoint)
        {
            ClearSelection(); // Clear previous selection

            // Create a world-space Bounds object from the two world points
            Bounds selectionBounds = new Bounds();
            selectionBounds.SetMinMax(
                new Vector3(Mathf.Min(startWorldPoint.x, endWorldPoint.x), -100f, Mathf.Min(startWorldPoint.z, endWorldPoint.z)),
                new Vector3(Mathf.Max(startWorldPoint.x, endWorldPoint.x), 100f, Mathf.Max(startWorldPoint.z, endWorldPoint.z))
            );
            // Adjust the Y values (-100f, 100f) to cover the vertical range of your units/buildings.

            Collider[] hits = Physics.OverlapBox(selectionBounds.center, selectionBounds.extents, Quaternion.identity, selectableLayer);

            foreach (Collider col in hits)
            {
                if (col.TryGetComponent(out ISelectable selectable))
                {
                    // For extra robustness, you could still check if the object's screen bounds intersect
                    // with the projected screen rectangle of the world selection.
                    // However, OverlapBox is often sufficient for practical RTS selection.
                    // If you want pixel-perfect selection even when the object is slightly off-screen
                    // but its world bounds technically intersect, you'd re-introduce
                    // IsObjectCompletelyOrPartiallyInScreenRect using the _projected_ screenRect.
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

        // You no longer need GetWorldBoundsFromScreenRect directly, as we're working with world points
        // The logic is moved into SelectUnitsInRectangle
    }
}