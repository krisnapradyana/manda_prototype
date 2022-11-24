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
    [field : SerializeField]
    public int CharacterId { get; private set; } 

    [SerializeField] bool _isSelected;
    //[SerializeField] float _speed;
    [SerializeField] AIPath _pathFinder;
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
                if (item.CameraId == CharacterId)
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