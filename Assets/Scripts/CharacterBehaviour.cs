using Cinemachine;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterBehaviour : MonoBehaviour
{
    public event Action<CharacterBehaviour> onHoverObject;
    public event Action<CharacterBehaviour> onExitHoverObject;
    public event Action<CharacterBehaviour> onInteractCharacter;
    public event Action onSelectCharacter;

    [field : SerializeField] public int CharacterId { get; private set; } 
    [field : SerializeField] public bool IsSelected { get; private set; }
    [field : SerializeField] public bool IsNPC { get; private set; }
    [field : SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field : SerializeField] public Seeker SeekerObject { get; private set; }
    [field : SerializeField] public AIPath AiPath { get; private set; }

    [SerializeField] float _runSpeed;
    [SerializeField] float _walkSpeed;

    [Header("References")]
    [SerializeField] GameHandler _gameHandler;
    [SerializeField] EventTrigger _eventTrigger;
    [SerializeField] Animator _characterAnimator;

    private float _targetDistance;
    private float _divider = 1;

    private void OnDestroy()
    {
        onSelectCharacter = null;
        onExitHoverObject = null;
        onHoverObject = null;
        onInteractCharacter = null;
    }

    void FixedUpdate()
    {
        _characterAnimator.SetFloat("Velocity", Mathf.Clamp( AiPath.velocity.magnitude, 0, 1)/_divider);
    }

    public void InitCharacterEvents(GameHandler handler)
    {
        _gameHandler = handler;
        _eventTrigger = GetComponent<EventTrigger>();

        _eventTrigger.AddEvent(EventTriggerType.PointerEnter, (data) =>
        {
            if (IsSelected)
            {
                return;
            }
            onHoverObject?.Invoke(this);
        });

        _eventTrigger.AddEvent(EventTriggerType.PointerExit, (data) =>
        {
            onExitHoverObject?.Invoke(this);
        });

        _eventTrigger.AddEvent(EventTriggerType.PointerClick, (data) =>
        {
            if (IsNPC)
            {
                Debug.Log("Scelected as NPC");

                onInteractCharacter?.Invoke(this);
                return;
            }

            if (IsSelected)
            {
                return;
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                onSelectCharacter?.Invoke();
                ToggleSelected(true);
                _gameHandler.SetCameraPriority(CharacterId);
                return;
            }
        });
    }

    public void ToggleSelected(bool state)
    {
        IsSelected = state;
    }

    public void MoveCharacter(Vector3 targetPosition)
    {
        _targetDistance = Vector3.Distance(_gameHandler.ControlledPlayer.transform.position, targetPosition);
        Debug.Log("Distance to Point : " + _targetDistance);
        if (_targetDistance > 4.5)
        {
            AiPath.maxSpeed = _runSpeed;
            _divider = 1;
        }
        else
        {
            AiPath.maxSpeed = _walkSpeed;
            _divider = 2;
        }
        SeekerObject.StartPath(_gameHandler.ControlledPlayer.transform.position, targetPosition);
    }
}