using Cinemachine;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [field : SerializeField] public int _characterId { get; private set; } 
    [field : SerializeField] public bool _isSelected { get; private set; }
    [field : SerializeField] public Rigidbody _rigidbody { get; private set; }
    //[SerializeField] float _speed;
    [field: SerializeField] public AIPath _pathFinder { get; private set; }
    [field: SerializeField] public Seeker _seekerObject { get; private set; }
    [SerializeField] GameHandler _gameHandler;
    [SerializeField] EventTrigger _eventTrigger;

    public event Action onMoveClicked;
    public event Action onSelectCharacter;

    public void InitCharacterEvents(GameHandler handler)
    {
        _gameHandler = handler;
        _eventTrigger = GetComponent<EventTrigger>();

        _eventTrigger.AddEvent(EventTriggerType.PointerClick, (data) =>
        {
            if (_isSelected)
            {
                return;
            }
            Debug.Log("Character : " + gameObject.name);
            onSelectCharacter?.Invoke();
            ToggleSelected(true);

            foreach (var item in _gameHandler._freelookCamera)
            {
                if (item.CameraId == _characterId)
                {
                    item.SetCameraPriority(1);
                }
            }
        });
    }

    public void ToggleSelected(bool state)
    {
        _isSelected = state;
    }
}