using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Pathfinding;
using Modules;

namespace Gameplay
{
    public class InputListener : MonoBehaviour
    {
        [SerializeField] GameHandler _gameHandler;
        [SerializeField] InputAction _moveAction;
        [SerializeField] InputAction _rotateAction;
        [SerializeField] InputAction _pointerInput;
        [SerializeField] GameObject _testCursor;

        Camera _cam;
        int layerMask = 1 << 9;
        bool held = false;

        private void Awake()
        {
            _moveAction.Enable();
            _rotateAction.Enable();
        }

        private void Start()
        {
            layerMask = ~layerMask;

            _cam = Camera.main;
            _moveAction.performed += context =>
            {
                MoveByMouse(AdditionalModule.GetWorldPoint());
            };

            _rotateAction.performed += context =>
            {
                held = true;
            };

            _rotateAction.canceled += context =>
            {
                held = false;
            };
        }

        private void Update()
        {
            DrawRayLine();
            RotateCamera(1000);
        }

        public void MoveByMouse(Vector3 targetPosition)
        {
            _testCursor.transform.position = new Vector3(targetPosition.x, 2, targetPosition.z);
            _gameHandler.ControlledPlayer.MoveCharacter(targetPosition);
        }

        void RotateCamera(float value)
        {
            if (!held)
            {
                return;
            }

            //if (Keyboard.current.eKey.isPressed)
            //{
            //    Debug.Log(value);
            //    _gameHandler.PriorityCamera.transform.RotateAround(_gameHandler.PriorityCamera.virtualCamera.Follow.transform.position , Vector3.up, value * Time.deltaTime);
            //}
            //else if(Keyboard.current.qKey.isPressed)
            //{
            //    Debug.Log(value  * -1);
            //    _gameHandler.PriorityCamera.transform.RotateAround(_gameHandler.PriorityCamera.virtualCamera.Follow.transform.position, Vector3.up, (value * -1) * Time.deltaTime);
            //}
        }

        void DrawRayLine()
        {
            Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
        }
    }
}