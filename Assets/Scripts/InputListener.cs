using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Pathfinding;

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
            MoveByMouse(GetWorldPoint());
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
        _gameHandler._controlledPlayer._seekerObject.StartPath(_gameHandler._controlledPlayer.transform.position, targetPosition);
    }

    private Vector3 GetWorldPoint()
    {
        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        Vector3 point = Vector3.zero;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Debug.LogFormat("Mouse Position : x{0}, y{1}, z{2} ", hit.point.normalized.x, hit.point.normalized.y, hit.point.normalized.z);
            point = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        return point;
    }
}
