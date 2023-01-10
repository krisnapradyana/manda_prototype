using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Pathfinding;
using Modules;
using Gameplay;
using UnityEngine.SceneManagement;
using System;

namespace Singletons
{
    public class InputListener : MonoBehaviour, Controls.IPlayerActionActions
    {
        public static InputListener Instance;

        [SerializeField] MainUI _mainUI;
        [SerializeField] GameHandler _gameHandler;
        [field: SerializeField] public InputAction UIAction { get; private set; }

        [HideInInspector] public Vector2 _mouseDelta;
        [HideInInspector] public Vector2 _moveComposite;

        Camera _cam;
        Controls _controls;
        int layerMask = 1 << 9;

        //Delegate Event Action
        public event Action<bool> OnToggledCursor;

        private void Awake()
        {
            if (Instance!= null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }    

            //MoveAction.Enable();
            UIAction.Enable();

            _cam = Camera.main;
        }

        private void OnEnable()
        {
            if (_controls != null)
                return;

            _controls = new Controls();
            _controls.PlayerAction.SetCallbacks(this);
            _controls.PlayerAction.Enable();
        }
        public void OnDisable()
        {
            _controls.PlayerAction.Disable();
        }

        private void Start()
        {
            layerMask = ~layerMask;

            SceneManager.activeSceneChanged += (last, curent) => _cam = Camera.main;

            UIAction.performed += context =>
            {
                _mainUI.SetupPopupUI("Notice", "Do you want to exit application?", yesButtonEnabled: true, noButtonEnabled: true);
                _mainUI.SetupUIEvents(yesAction: () => { Application.Quit(); print("has quit application"); });
                StartCoroutine(_mainUI.ShowPopupIE(() => print("has shown popup")));
            };
        }

        private void Update()
        {
            DrawRayLine();
        }

        public void InitGameHandler(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
        }

        public void MoveByMouse(Vector3 targetPosition)
        {
            _gameHandler.ControlledPlayer.MoveCharacter(targetPosition);
        }

        void DrawRayLine()
        {
            Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
        }

        public void OnLook(InputAction.CallbackContext context)
        {   
            _mouseDelta = context.ReadValue<Vector2>();
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            _moveComposite = context.ReadValue<Vector2>();
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            return;
        }

        public void OnMoveByMouse(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            if (_gameHandler._centralSystem.CurrentState == GameState.gameplay && _gameHandler.ControlledPlayer != null)
            {
                MoveByMouse(AdditionalModule.GetWorldPoint());
            }
        }

        public void OnToggleMouseLock(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                var lastState = Cursor.lockState;
                Debug.Log("Alt Pressed");
                switch (lastState)
                {
                    case CursorLockMode.None: Cursor.lockState = CursorLockMode.Locked;
                        _controls.PlayerAction.Look.Enable();
                        OnToggledCursor?.Invoke(false);
                        Debug.Log("Look Control : " + _controls.PlayerAction.Look.enabled);
                        break;
                    case CursorLockMode.Locked: Cursor.lockState = CursorLockMode.None;
                        _controls.PlayerAction.Look.Disable();
                        OnToggledCursor?.Invoke(true);
                        Debug.Log("Look Control : " + _controls.PlayerAction.Look.enabled);
                        break;
                }
            }
        }
    }
}