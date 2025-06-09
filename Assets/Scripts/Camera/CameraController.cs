using System;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        [SerializeField][Header("Camera movement Settings")]
        private Transform mainCamera;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float scrollSpeed = 50f;//TODO: need a settings manager and/or panel
        [SerializeField] private float borderScrollSpeed = 10f;
        [SerializeField] [Range(0f, 0.2f)] 
        private float borderThicknessPercent = 0.05f; 
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 25f;
        
        [SerializeField][Header("Input Actions")]
        private InputActionAsset playerInputActions; //TODO: mb better to use a service locator
        
        [SerializeField][Header("Focusing Target")]
         private Transform mainHero;
        [SerializeField] private float focusLerpSpeed = 5f;
        
        private InputAction panCameraAction;
        private InputAction zoomCameraAction;
        private InputAction toggleFocusAction;
        
        private Vector2 currentMouseScreenInput;
        private Vector2 currentMousePosition;
        private float currentZoomInput;
        private float heroZdeviation;
        private bool heroInFocus = true;

        private float borderThicknessX;
        private float borderThicknessY;
        
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<ICameraController>(this);
        }
        
        private void Start()
        {
            InputActionMap cameraActionMap = playerInputActions.FindActionMap("Gameplay"); //TODO: add a static class with names of a manager for game inputs with it

            panCameraAction = cameraActionMap.FindAction("MouseScreenPos");//TODO: add a static class with names of a manager for game inputs with it
            zoomCameraAction = cameraActionMap.FindAction("ZoomCamera");//TODO: add a static class with names of a manager for game inputs with it
            toggleFocusAction = cameraActionMap.FindAction("ToggleFocus");//TODO: add a static class with names of a manager for game inputs with it
            
            panCameraAction.performed += OnMouseMove;
            panCameraAction.canceled += OnMouseMove; 
            
            zoomCameraAction.performed += OnZoomCamera;
            zoomCameraAction.canceled += OnZoomCamera;
            
            toggleFocusAction.performed += OnToggleFocus;
            heroInFocus = true;
            heroZdeviation = (maxZoom - minZoom) / 2f;
            
            borderThicknessX = Screen.width * borderThicknessPercent;
            borderThicknessY = Screen.height * borderThicknessPercent;
            
            panCameraAction.Enable();
            zoomCameraAction.Enable();
            toggleFocusAction.Enable();
        }
        
        private void Update()
        {
            HandleCameraMovement();
            HandleZoom();
        }
        
        private void OnMouseMove(InputAction.CallbackContext context)
        {
            currentMouseScreenInput = context.ReadValue<Vector2>();
        }
        
        private void OnZoomCamera(InputAction.CallbackContext context)
        {
            currentZoomInput = context.ReadValue<Vector2>().y;
        }
        
        private void OnToggleFocus(InputAction.CallbackContext context)
        {
            heroInFocus = !heroInFocus;
        }
        
        private void HandleCameraMovement() //TODO: add borders
        {
            Vector3 moveDirection = Vector3.zero;

            if (heroInFocus)
            {
                FollowTheHero();
                return;
            }
            
            if (currentMouseScreenInput.x < borderThicknessX)
            {
                moveDirection.x -= 1;
            }
            else if (currentMouseScreenInput.x > Screen.width - borderThicknessX)
            {
                moveDirection.x += 1;
            }

            if (currentMouseScreenInput.y < borderThicknessY)
            {
                moveDirection.z -= 1;
            }
            else if (currentMouseScreenInput.y > Screen.height - borderThicknessY)
            {
                moveDirection.z += 1;
            }
            
            if (moveDirection.magnitude > 0) //TODO: add some threshold
            {
                mainCamera.transform.position += moveDirection.normalized * (moveSpeed * Time.deltaTime); //TODO: add a time manager
            }
        }

        private void FollowTheHero()
        {
            Vector3 cameraForward = mainCamera.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
                
            Vector3 desiredPosition = mainHero.position - cameraForward * heroZdeviation;
            desiredPosition.y = mainCamera.transform.position.y;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,
                desiredPosition, Time.deltaTime * focusLerpSpeed); //TODO: smoothing formula  a = b + (a - b) * exp( - decayConst * Time.deltaTime) 
        }

        private void HandleZoom()
        {
            Vector3 zoomDirection = mainCamera.forward * currentZoomInput;
            mainCamera.position -= zoomDirection * (scrollSpeed * Time.deltaTime);
            
            mainCamera.position = new Vector3(mainCamera.position.x,
                Mathf.Clamp(mainCamera.position.y, minZoom, maxZoom),
                mainCamera.position.z);
            
            heroZdeviation += currentZoomInput * scrollSpeed * Time.deltaTime;
            heroZdeviation = Mathf.Clamp(heroZdeviation, minZoom, maxZoom);
        }

        private void OnDestroy()
        {
            panCameraAction.performed -= OnMouseMove;
            panCameraAction.canceled -= OnMouseMove;

            zoomCameraAction.performed -= OnZoomCamera;
            zoomCameraAction.canceled -= OnZoomCamera;

            toggleFocusAction.performed -= OnToggleFocus;

            panCameraAction.Disable();
            zoomCameraAction.Disable();
            toggleFocusAction.Disable();
            ServiceLocator.Instance.Unregister<ICameraController>();
        }
    }
}
