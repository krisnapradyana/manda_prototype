using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace Gameplay
{
    public class GameHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GameplayUIControl _uiControl;
        [SerializeField] GameObject _playgroundParent;
        [SerializeField] InspectGroundBehaviour _inspectParent;

        [Header("Getter Setter Fields")]
        [field: SerializeField] public CharacterBehaviour[] Players;
        [field: SerializeField] public CameraCore[] Cameras;
        [field: SerializeField] public ObjectBehaviour[] Buildings;
        [field: SerializeField] public ObjectBehaviour[] Objects;
        public CharacterBehaviour ControlledPlayer
        {
            get
            {
                CharacterBehaviour result = null;
                foreach (var item in Players)
                {
                    if (item.IsSelected)
                    {
                        result = item;
                    }
                }
                return result;
            }
        }

        public bool IsInspecting { get; private set; }
        public CameraCore PriorityCamera { get; private set; }

        //Singleton privates
        GameDataContainer _dataContainer;

        ///Private fields
        CharacterBehaviour _inspectedCharacter;
        ObjectBehaviour _inspectedObject;
        Quaternion _selectedRotationData;
        int _lastCameraPriority;
        bool _hasInspectCharacter;

        private void Start()
        {
            Debug.Log("Starting handler");
            _dataContainer = FindObjectOfType<GameDataContainer>();
            InitObjects();
            if (_dataContainer == null)
            {
                InitEvents(0);
            }
            else
            {
                InitEvents(_dataContainer.SelectedCharacterIndex);
            }
        }

        private void Update()
        {
            MatchCameraRotation();
        }

        private void InitObjects()
        {
            foreach (var item in Buildings)
            {
                Debug.Log("Initializing Objects");
                item.onHoverObject += (info) => _uiControl.ToggleHoverInfo(info.gameObject).ToggleMouse(info.Acquire<CharacterBehaviour>(), _uiControl.MousePivot);
                item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo();
                item.onInteractObject += (info) => info.Acquire<ObjectBehaviour>().OnObjectInspected(out _inspectedObject, out _selectedRotationData, _inspectParent.gameObject, _playgroundParent, _uiControl, () =>
                {
                    AsisgnCameraPriority(3, false);
                    _hasInspectCharacter = false;
                    _inspectParent.ToggleInspectionBackground(false);
                    IsInspecting = true;
                });
            }

            foreach (var item in Objects)
            {
                item.onHoverObject += (info) => _uiControl.ToggleHoverInfo(info.gameObject).ToggleMouse(info.Acquire<CharacterBehaviour>(), _uiControl.MousePivot);
                item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo();
            }
        }

        private void InitEvents(int focusCharacters)
        {
            Debug.Log("Initializing character events");
            _uiControl.onReturnInspectPressed += () =>
            {
                if (_hasInspectCharacter)
                {
                    _inspectedCharacter.ExitInspectCharacter(_selectedRotationData, _inspectParent.gameObject, _playgroundParent, _uiControl, () =>
                    {
                        AsisgnCameraPriority(_lastCameraPriority, false);
                    });
                }
                else
                {
                    _inspectedObject.ExitInspectObject(_selectedRotationData, _inspectParent.gameObject, _playgroundParent, _uiControl, () =>
                    {
                        AsisgnCameraPriority(_lastCameraPriority, false);
                    });
                }
                IsInspecting = false;
            };

            foreach (var item in Players)
            {
                Debug.Log("Creating Character event");
                item.InitCharacterEvents(this);
                item.onHoverObject += (info) => _uiControl.ToggleHoverInfo(info.gameObject).ToggleMouse(info.Acquire<CharacterBehaviour>(), _uiControl.MousePivot);
                item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo();
                item.onSelectCharacter += OnChangedCharacted;
                item.onInteractObject += (info) => info.Acquire<CharacterBehaviour>().OnCharacterInspected(out _inspectedCharacter, out _selectedRotationData, _inspectParent.gameObject, _playgroundParent, _uiControl, () =>
                {
                    AsisgnCameraPriority(3, false);
                    _hasInspectCharacter = true;
                    _inspectParent.ToggleInspectionBackground(true);
                    IsInspecting = true;
                });
            }

            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].CharacterId == focusCharacters)
                {
                    Players[i].SetSelected(true);
                }
            }
        }

        public void OnChangedCharacted()
        {
            //Debug.Log("Changed character");
            ResetAllVirtualCameraPriority();
            foreach (var item in Players)
            {
                item.SetSelected(false);
            }
        }

        public void AsisgnCameraPriority(int comparedId, bool saveLastId = true)
        {
            ResetAllVirtualCameraPriority();
            foreach (var item in Cameras)
            {
                if (item.CameraId == comparedId)
                {
                    item.SetCameraPriority(1);
                    PriorityCamera = item;
                    if (saveLastId)
                        _lastCameraPriority = comparedId;
                }
            }
        }

        public void ResetAllVirtualCameraPriority()
        {
            foreach (var item in Cameras)
            {
                item.SetCameraPriority(0);
            }
        }

        void MatchCameraRotation()
        {
            foreach (var item in Cameras)
            {
                item.OnCameraRotation(PriorityCamera.CameraRotationValue);
            }
        }
    }
}