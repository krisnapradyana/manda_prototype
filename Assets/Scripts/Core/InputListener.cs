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
        [SerializeField] InputAction _pointerInput;
        [SerializeField] GameObject _testCursor;

        Camera _cam;
        int layerMask = 1 << 9;

        private void Awake()
        {
            _moveAction.Enable();
        }

        private void Start()
        {
            layerMask = ~layerMask;

            _cam = Camera.main;
            _moveAction.performed += context =>
            {
                MoveByMouse(AdditionalModule.GetWorldPoint());
            };
        }

        private void Update()
        {
            //GetWorldPoint();
            Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
        }

        public void MoveByMouse(Vector3 targetPosition)
        {
            _testCursor.transform.position = new Vector3(targetPosition.x, 2, targetPosition.z);
            _gameHandler.ControlledPlayer.MoveCharacter(targetPosition);
        }
    }
}