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
        [field: SerializeField] public bool HasAI { get; private set; }
        [field: SerializeField] public bool IsVRCharacter { get; private set; }
        [field: SerializeField] public SkinContainer[] CharacterSkins { get; private set; }

        [SerializeField] float _runSpeed;
        [SerializeField] float _walkSpeed;

        [Header("References")]
        [SerializeField] EventTrigger _eventTrigger;
        public DoOnce doOnce = new DoOnce();

        private int _skinIndex;
        private float _targetDistance;
        private float _divider = 1;

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private IEnumerator Start()
        {
            if (IsNPC)
            {
                yield break;
            }

            yield return new WaitForEndOfFrame();
            //EnableSkin(_gameHandler.centralSystem.SelectedCharacterIndex);
        }

        void FixedUpdate()
        {
            if (IsNPC)
            {
                return;
            }
            
            if(!HasAI) { return; }
            CharacterSkins[_skinIndex].Animation.SetFloat("Velocity", Mathf.Clamp(AiPath.velocity.magnitude, 0, 1) / _divider);
        }

        public void InitCharacterEvents(object handler)
        {
            _gameHandler = handler as RootHandler;
            _eventTrigger = GetComponent<EventTrigger>();

            doOnce.SetDoOnceAction(() => onHoverObject?.Invoke(this));

            _eventTrigger.AddEvent(EventTriggerType.PointerEnter, (data) =>
            {
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
                        //Character will talk here
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

            ///Main handler related, Should rework the mechanism
            _gameHandler.ResetAllVirtualCameraPriority(_gameHandler._worldCameras);
            _gameHandler.AssignCameraPriority(CharacterId, _gameHandler._worldCameras); //Curreently do nothing
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

        private void EnableSkin(int skinIndex)
        {
            _skinIndex = skinIndex;
            foreach (var item in CharacterSkins)
            {
                item.gameObject.SetActive(false);
            }

            CharacterSkins[skinIndex].gameObject.SetActive(true);
        }
    }
}