using Cinemachine;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class CharacterBehaviour : Interactables
    {
        [field: Space]
        [field: Header("Character Reference")]
        [field: SerializeField] public int CharacterId { get; private set; }
        [field: SerializeField] public bool IsSelected { get; private set; }
        [field: SerializeField] public bool IsNPC { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Seeker SeekerObject { get; private set; }
        [field: SerializeField] public AIPath AiPath { get; private set; }

        [SerializeField] float _runSpeed;
        [SerializeField] float _walkSpeed;

        [Header("References")]
        [SerializeField] EventTrigger _eventTrigger;
        [SerializeField] Animator _characterAnimator;

        private float _targetDistance;
        private float _divider = 1;

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        void FixedUpdate()
        {
            _characterAnimator.SetFloat("Velocity", Mathf.Clamp(AiPath.velocity.magnitude, 0, 1) / _divider);
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
                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    if (IsNPC)
                    {
                        Debug.Log("Scelected as NPC");

                        if (_gameHandler.IsInspecting)
                        {
                            return;
                        }
                        onInteractObject?.Invoke(this);
                        return;
                    }

                    if (IsSelected)
                    {
                        return;
                    }

                    onInteractObject?.Invoke(this);
                    Debug.Log(_gameHandler.ControlledPlayer.name);
                    return;
                }
            });
        }

        public void SetSelected( bool state)
        {
            Debug.LogFormat("Set character {0} to {1} ", gameObject.name, state);
            IsSelected = state;
            _gameHandler.AsisgnCameraPriority(CharacterId);
        }

        public void MoveCharacter(Vector3 targetPosition)
        {
            _targetDistance = Vector3.Distance(_gameHandler.ControlledPlayer.transform.position, targetPosition);
            Debug.Log("Distance to Point : " + _targetDistance);
            if (_targetDistance > 6)
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
}