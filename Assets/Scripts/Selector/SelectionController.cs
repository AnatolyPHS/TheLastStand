using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Selector
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField][Header("Input Actions")]
        private InputActionAsset playerInputActions; //TODO: use service locator
        
        [SerializeField][Header("UI Reference")]
        private RectTransform selectionRectangleUI; //TODO: add to separate view
        
        [SerializeField][Header("Selection Settings")]
        private LayerMask selectableLayer;
        [SerializeField] private UnityEngine.Camera mainCamera; //TODO: use servicde locator and take it from CameraMover
        [SerializeField] private float minDragDistance = 5f;
        
        private InputAction leftClickAction;
        private InputAction mousePositionAction;
        
        private bool isDragging = false;
        private Vector2 startMousePosition;
        private Vector2 currentMousePosition;

        private List<ISelectable> currentlySelectedObjects = new List<ISelectable>();
        
        private void Awake()
        {
            var gameplayActionMap = playerInputActions.FindActionMap("Gameplay");
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
            if (isDragging == false)
            {
                return;
            }
         
            currentMousePosition = mousePositionAction.ReadValue<Vector2>();
            UpdateSelectionRectangleUI();
        }
        
        private void UpdateSelectionRectangleUI() 
        {
            if (Vector2.Distance(startMousePosition, currentMousePosition) < minDragDistance) //TODO: add the below logic to view
            {
                selectionRectangleUI.gameObject.SetActive(false);
                return;
            }

            if (selectionRectangleUI.gameObject.activeSelf == false)
            {
                selectionRectangleUI.gameObject.SetActive(true);
            }

            float x1 = Mathf.Min(startMousePosition.x, currentMousePosition.x);
            float y1 = Mathf.Min(startMousePosition.y, currentMousePosition.y);
            float x2 = Mathf.Max(startMousePosition.x, currentMousePosition.x);
            float y2 = Mathf.Max(startMousePosition.y, currentMousePosition.y);

            selectionRectangleUI.position = new Vector2(x1, y1);
            selectionRectangleUI.sizeDelta = new Vector2(x2 - x1, y2 - y1);
        }
        
        
        private void OnLeftClickStarted(InputAction.CallbackContext context)
        {
            currentMousePosition = mousePositionAction.ReadValue<Vector2>();
            startMousePosition = currentMousePosition;
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
            selectionRectangleUI.gameObject.SetActive(false); //TODO: add to view
            
            if (Vector2.Distance(startMousePosition, currentMousePosition) < minDragDistance)
            {
                HandleSingleClickSelection();
            }
            else
            {
                SelectUnitsInRectangle(startMousePosition, currentMousePosition);
            }
        }
        
        private void SelectUnitsInRectangle(Vector2 startScreenPos, Vector2 endScreenPos)
        {
            Rect selectionRect = new Rect(
                Mathf.Min(startScreenPos.x, endScreenPos.x),
                Mathf.Min(startScreenPos.y, endScreenPos.y),
                Mathf.Abs(startScreenPos.x - endScreenPos.x),
                Mathf.Abs(startScreenPos.y - endScreenPos.y)
            );
            
            Collider[] allSelectables = Physics.OverlapSphere(mainCamera.transform.position, 1000f, selectableLayer); //TODO: use camera frustrum
            
            foreach (Collider col in allSelectables)
            {
                if (col.TryGetComponent(out ISelectable selectable) == false) //TODO: check for sinle selection only
                {
                    continue;
                }

                Vector2 screenPosition = mainCamera.WorldToScreenPoint(selectable.GetVisualPosition());
                
                if (selectionRect.Contains(screenPosition) == false)
                {
                    continue;
                }
                
                SelectObject(selectable);
            }
        }
        
        private void HandleSingleClickSelection()
        {
            Ray ray = mainCamera.ScreenPointToRay(currentMousePosition);
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
            if (currentlySelectedObjects.Contains(selectable) == false)
            {
                selectable.OnSelect(); // Assuming ISelectable has an OnSelect method
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
