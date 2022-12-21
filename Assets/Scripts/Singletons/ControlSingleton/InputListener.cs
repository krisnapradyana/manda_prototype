using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Pathfinding;
using Modules;
using Gameplay;
using UnityEngine.SceneManagement;

namespace Singletons
{
    public class InputListener : MonoBehaviour
    {
        public static InputListener Instance;

        [SerializeField] GameHandler _gameHandler;
        [SerializeField] PopupUI _popupUI;
        [field : SerializeField] public InputAction MoveAction {get; private set;}
        [field: SerializeField] public InputAction UIAction { get; private set; }
        [field: SerializeField] public InputAction wasdMoveAction { get; private set; }

        Camera _cam;
        int layerMask = 1 << 9;

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

            MoveAction.Enable();
            UIAction.Enable();

            _cam = Camera.main;
        }

        private void Start()
        {
            layerMask = ~layerMask;

            SceneManager.activeSceneChanged += (last, curent) => _cam = Camera.main;
            MoveAction.performed += context =>
            {
                if (_gameHandler._gameDataContainer.CurrentState == GameState.gameplay && _gameHandler.ControlledPlayer != null)
                {
                    MoveByMouse(AdditionalModule.GetWorldPoint());
                }
            };

            wasdMoveAction.performed += context =>
            {
                Debug.Log(context.ReadValue<Vector2>());
            };

            UIAction.performed += context =>
            {
                _popupUI.SetupPopupUI("Notice", "Do you want to exit application?", yesButtonEnabled: true, noButtonEnabled: true);
                _popupUI.SetupUIEvents(yesAction: () => { Application.Quit(); print("has quit application"); });
                StartCoroutine(_popupUI.ShowPopup(() => print("has shown popup")));
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
    }
}