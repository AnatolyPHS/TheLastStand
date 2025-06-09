using Camera;
using Services;
using UnityEngine;

namespace UI.SelectorView
{
    public class SelectorView : MonoBehaviour, ISelectorView
    {
        [SerializeField] private RectTransform selectionRectangleUI;
        
        private UnityEngine.Camera mainCamera;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<ISelectorView>(this);
        }

        private void Start()
        {
            mainCamera = ServiceLocator.Instance.Get<ICameraController>().GetCamera();
        }
        
        public void SetSelectorState(bool newState)
        {
            selectionRectangleUI.gameObject.SetActive(newState);
        }
        
        public void UpdateSelector(Vector3 startWorldPoint, Vector3 currentWorldPoint, float minDragDistance)
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

        private void OnDestroy()
        {
            ServiceLocator.Instance.Unregister<ISelectorView>();
        }
    }
}
