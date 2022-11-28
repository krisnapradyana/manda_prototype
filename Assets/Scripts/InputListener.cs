using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Pathfinding;

public class InputListener : MonoBehaviour
{
    [SerializeField] GameHandler _gameHandler;
    [SerializeField] InputAction _moveAction;
    [SerializeField] InputAction _pointerPos;

    private void Awake()
    {
        _moveAction.Enable();
        _pointerPos.Enable();
    }

    private void Start()
    {
        _moveAction.performed += context =>
        {
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = Camera.main.nearClipPlane;
            MoveByMouse(position);
            //Debug.LogFormat("Mouse Position : x{0}, y{1} ", position.x, position.y);
        };
    }

    private void Update()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = Camera.main.nearClipPlane;
        Debug.LogFormat("Mouse Position : x{0}, y{1} ", position.x, position.y, position.z);
    }

    public void MoveByMouse(Vector2 targetPosition)
    {
        //_gameHandler._controlledPlayer._pathFinder.Move(new Vector3(targetPosition.x, _gameHandler._controlledPlayer.transform.position.y, targetPosition.y));
        _gameHandler._controlledPlayer._seekerObject.StartPath(_gameHandler._controlledPlayer.transform.position, targetPosition);
    }

    public void MoveByKey()
    {
        
    }
}
