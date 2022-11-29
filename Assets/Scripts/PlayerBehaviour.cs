﻿using Cinemachine;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [field : SerializeField] public int CharacterId { get; private set; } 
    [field : SerializeField] public bool IsSelected { get; private set; }
    [field : SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public Seeker SeekerObject { get; private set; }
    [field: SerializeField] public AIPath AiPath { get; private set; }

    [Header("References")]
    [SerializeField] GameHandler _gameHandler;
    [SerializeField] EventTrigger _eventTrigger;
    [SerializeField] Animator _characterAnimator;

    //public event Action onMoveClicked;
    public event Action onSelectCharacter;

    void FixedUpdate()
    {
        _characterAnimator.SetFloat("Velocity", AiPath.velocity.magnitude);
    }

    public void InitCharacterEvents(GameHandler handler)
    {
        _gameHandler = handler;
        _eventTrigger = GetComponent<EventTrigger>();

        _eventTrigger.AddEvent(EventTriggerType.PointerClick, (data) =>
        {
            if (IsSelected)
            {
                return;
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                Debug.Log("Trying to select character : "+ gameObject.name);
                Debug.Log("Character : " + gameObject.name);
                onSelectCharacter?.Invoke();
                ToggleSelected(true);

                foreach (var item in _gameHandler._cameras)
                {
                    if (item.CameraId == CharacterId)
                    {
                        item.SetCameraPriority(1);
                    }
                }
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
        SeekerObject.StartPath(_gameHandler._controlledPlayer.transform.position, targetPosition);
    }
}