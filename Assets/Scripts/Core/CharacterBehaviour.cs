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
    public class CharacterBehaviour : MonoBehaviour, InteractableObject
    {
        public event Action onSelectCharacter;
        [field: SerializeField] public int CharacterId { get; private set; }
        [field: SerializeField] public bool IsSelected { get; private set; }
        [field: SerializeField] public bool IsNPC { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Seeker SeekerObject { get; private set; }
        [field: SerializeField] public AIPath AiPath { get; private set; }
        public EventTrigger Trigger { get; set; }
        public bool IsInspectable { get; set; }

        [SerializeField] float _runSpeed;
        [SerializeField] float _walkSpeed;

        [Header("References")]
        [SerializeField] GameHandler _gameHandler;
        [SerializeField] EventTrigger _eventTrigger;
        [SerializeField] Animator _characterAnimator;

        private float _targetDistance;
        private float _divider = 1;

        public event Action<GameObject> onHoverObject;
        public event Action<GameObject> onExitHoverObject;
        public event Action<GameObject> onInteractObject;

        private void OnDestroy()
        {
            onSelectCharacter = null;
            onExitHoverObject = null;
            onHoverObject = null;
            onInteractObject = null;
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
                onHoverObject?.Invoke(gameObject);
            });

            _eventTrigger.AddEvent(EventTriggerType.PointerExit, (data) =>
            {
                onExitHoverObject?.Invoke(gameObject);
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
                        onInteractObject?.Invoke(gameObject);
                        return;
                    }

                    if (IsSelected)
                    {
                        return;
                    }

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