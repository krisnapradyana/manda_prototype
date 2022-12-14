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
        [field: SerializeField] public Interactables InspectedObject { get => _inspectedObject; }
        public bool IsInspecting { get; private set; }
        public CameraCore PriorityCamera { get; private set; }

        [Header("WIP")]
        public int _playerGold;

        //Singleton privates
        [HideInInspector] public GameDataContainer _dataContainer { get; private set; }

        ///Private fields
        Interactables _inspectedObject;
        int _lastCameraPriority;
        bool _hasInspectCharacter;

        private void Start()
        {
            Debug.Log("Starting handler");
            _dataContainer = FindObjectOfType<GameDataContainer>();
            InitObjects();
            _uiControl.SetPlayerUI();
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
            foreach (var item in Objects)
            {
                Debug.Log("Initializing Objects");
                item.onHoverObject += (info) => {
                    if (IsInspecting)
                    {
                        _uiControl.ToggleHoverInfo();
                        return;
                    }

                    var objInfo = info;
                    _uiControl.ToggleHoverInfo(info.gameObject).ToggleMouse(objInfo.GetComponent<ObjectBehaviour>(), _uiControl.MousePivot
                        );
                };
                item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo();
                item.onInteractObject += InspectObj;
                item.onlevelUp += () =>
                {
                    var platformData = _inspectedObject.gameObject.GetComponent<ObjectBehaviour>().PlatformData;
                    var platformInteractable = _inspectedObject.gameObject.GetComponent<Interactables>();
                    _uiControl.SetUpdateObjectDescription(platformData.platformName, platformData.platformDescription, string.Format("Level : {0}", _inspectedObject.Level.ToString())).SetLevelUpButton(platformInteractable.Level, platformInteractable.MaxLevel);
                    item.ShowObjectProperties();
                };
            }

            _uiControl.onReturnInspectPressed += () =>
            {
                _inspectedObject.ExitInspectObject(_inspectParent.gameObject, _playgroundParent, _uiControl, () =>
                {
                    AsisgnCameraPriority(_lastCameraPriority, false);
                });

                IsInspecting = false;
            };
        }

        private void InitEvents(int focusCharacters)
        {
            Debug.Log("Initializing character events");

            foreach (var item in Players)
            {
                Debug.Log("Creating Character event");
                item.InitCharacterEvents(this);
                item.onHoverObject += (info) => {
                    if (IsInspecting)
                    {
                        _uiControl.ToggleHoverInfo();
                        return;
                    }

                    if (info.GetComponent<CharacterBehaviour>().IsSelected)
                    {
                        print("hover on selected");
                        _uiControl.ToggleHoverInfo(info.gameObject, customObjName: "You", showMouse: false).ToggleMouse(info.GetComponent<CharacterBehaviour>(), _uiControl.MousePivot);
                    }
                    else
                    {
                        _uiControl.ToggleHoverInfo(info.gameObject).ToggleMouse(info.GetComponent<CharacterBehaviour>(), _uiControl.MousePivot);
                    }
                };
                item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo();
                item.onInteractObject += (info) => { OnChangedCharacted(); info.GetComponent<CharacterBehaviour>().SetSelected(true); };
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

        void InspectObj(Interactables interactables)
        {
            _uiControl.ToggleHoverInfo();
            interactables.OnObjectInspected(out _inspectedObject, _inspectParent.gameObject, _playgroundParent, _uiControl, () =>
            {
                _uiControl.SetLevelUpButton(interactables.Level, interactables.MaxLevel);
                OnInspecting(interactables);
            });
        }

        void OnInspecting(Interactables intereactedObj)
        {
            switch (intereactedObj.Type)
            {
                case ObjectType.Character:
                    AsisgnCameraPriority(intereactedObj.gameObject.GetComponent<CharacterBehaviour>().CharacterId, true);
                    OnChangedCharacted();
                    break;
                case ObjectType.Object:
                    Debug.Log("Inspecting Object");
                    var platformData = _inspectedObject.gameObject.GetComponent<ObjectBehaviour>().PlatformData;
                    var platformInteractable = _inspectedObject.gameObject.GetComponent<Interactables>();
                    _uiControl.SetUpdateObjectDescription(platformData.platformName,platformData.platformDescription, string.Format("Level : {0}", _inspectedObject.Level.ToString()))
                        .SetLevelUpButton(platformInteractable.Level, platformInteractable.MaxLevel);
                    AsisgnCameraPriority(3, false);
                    _inspectParent.ToggleInspectionBackground(true);
                    IsInspecting = true;
                    break;
                default:
                    break;
            }
        }
    }
}