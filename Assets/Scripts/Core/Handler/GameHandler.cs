using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Singletons;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class GameHandler : MonoBehaviour
    {
        //Singleton privates
        [HideInInspector] public GameCentralSystem _centralSystem { get; private set; }
        [HideInInspector] public MainUI _mainUI { get; private set; }
        [HideInInspector] public InputListener _inputListener { get; private set; }

        [Header("Scene Root object")]
        [SerializeField] GameObject _rootScene;
        [SerializeField] GameObject _indoorScene;

        [Header("References")]
        [SerializeField] GameplayUIControl _uiControl;
        [SerializeField] GameObject _playgroundParent;
        [SerializeField] InspectGroundBehaviour _inspectParent;

        [Header("Third person inspect")]
        [SerializeField] private ThirdPersonPlayerControl _thirdPersonCharacter;
        [SerializeField] private GenericTrigger _inspectExit;
        [SerializeField] private Transform _inspectStartPos;

        [Header("Getter Setter Fields")]
        public CharacterBehaviour[] _players;
        public CameraCore[] _worldCameras;
        public CameraCore[] _inspectCameras; 
        public ObjectBehaviour[] _objects;
        public CharacterBehaviour ControlledPlayer
        {
            get
            {
                CharacterBehaviour result = null;
                foreach (var item in _players)
                {
                    if (item.IsSelected)
                    {
                        result = item;
                    }
                }
                return result;
            }
        }
        public Interactables InspectedObject { get => _inspectedObject; }
        public bool IsInspecting { get; private set; }
        public CameraCore PriorityCamera { get; private set; }

        [Header("Inspect Attributes")]
        [SerializeField] Interactables _lookTarget;


        ///Private fields
        Interactables _inspectedObject;
        //int _lastCameraPriority;

        private void OnDestroy()
        {
          
        }

        private void Awake()
        {            
            Debug.Log("Starting handler");
            _centralSystem = FindObjectOfType<GameCentralSystem>();
            _mainUI = FindObjectOfType<MainUI>();
            _inputListener = FindObjectOfType<InputListener>();
        }

        private void Start()
        {
            _inputListener.InitGameHandler(this);
            InitObjects();
            _uiControl.SetPlayerUI();
            if (_centralSystem == null)
            {
                InitEvents();//0);
            }
            else
            {
                InitEvents();//_centralSystem.SelectedCharacterIndex);
            }

            _centralSystem.SetGameState(GameState.gameplay);
            _inspectExit.InitTrigger(_thirdPersonCharacter.gameObject.tag, () =>
            {
                _mainUI.SetupPopupUI("Exit", "Do you want to return to world?", yesButtonEnabled: true, noButtonEnabled: true);
                _mainUI.SetupUIEvents(yesAction: () =>
                {
                    ExitVisitRoom();
                },
                    noAction: () => _centralSystem.SetGameState(GameState.inspect));
                StartCoroutine(_mainUI.ShowPopupIE(() => _centralSystem.SetGameState(GameState.none)));
            });
        }

        private void Update()
        {
            //MatchCameraRotation(_worldCameras);
        }

        private void InitObjects()
        {
            foreach (var item in _objects)
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

            //_uiControl.onReturnInspectPressed += () =>
            //{
            //    _inspectedObject.ExitInspectObject(_inspectParent.gameObject, _playgroundParent, _uiControl, () =>
            //    {
            //        ResetAllVirtualCameraPriority(_worldCameras);
            //        AssignCameraPriority(_lastCameraPriority, _worldCameras, false);
            //    });
            //
            //    IsInspecting = false;
            //};
        }

        private void InitEvents()//int focusCharacters)
        {
            Debug.Log("Initializing character events");

            foreach (var item in _players)
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
                item.onInteractObject += (info) =>
                {
                    {
                        if (item.IsNPC)
                        {
                            if (_mainUI._isSpeaking)
                            {
                                Debug.Log("character currently speaking");
                                return;
                            }
                            _mainUI.ShowDialogWindow(item.name, item.transform, item._cameraTransform.position, item.GetDialogData(),
                                () => Debug.Log("Yes Pressed"),
                                () => Debug.Log("No Pressed")
                                );
                        }
                        else
                        {
                            OnChangedCharacted(); item.SetSelected(true);
                        }
                    }
                };
            }

            //for (int i = 0; i < _players.Length; i++)
            //{
            //    if (_players[i].CharacterId == focusCharacters)
            //    {
            //        _players[i].SetSelected(true);
            //    }
            //}
        }

        public void OnChangedCharacted()
        {
            ResetAllVirtualCameraPriority(_worldCameras);
            foreach (var item in _players)
            {
                item.SetSelected(false);
            }
        }

        public void AssignCameraPriority(int comparedId, CameraCore[] collectionList)//, bool saveLastId = true)
        {
            foreach (var item in collectionList)
            {
                if (item.CameraId == comparedId)
                {
                    item.SetCameraPriority(1);
                    PriorityCamera = item;
                }
            }
        }

        public void ResetAllVirtualCameraPriority(CameraCore[] collectionList)
        {
            foreach (var item in collectionList)
            {
                item.SetCameraPriority(0);
            }
        }

        void MatchCameraRotation(CameraCore[] collectionList)
        {
            if (collectionList.Length < 1)
            {
                return;
            }

            foreach (var item in collectionList)
            {
                item.OnCameraRotation(PriorityCamera.CameraRotationValue);
            }
        }

        void InspectObj(Interactables interactables)
        {
            _uiControl.ToggleHoverInfo();
            interactables.OnObjectInspected(out _inspectedObject, _inspectParent.gameObject, _playgroundParent, _uiControl, () =>
            {
                _mainUI.FadeScreen(true, .5f, () => _mainUI.ToggleBlockScreen(true), () =>
                {
                    _rootScene.SetActive(false);
                    _indoorScene.SetActive(true);
                    _centralSystem.SetGameState(GameState.none);
                    _mainUI.FadeScreen(false, .5f);
                    ResetAllVirtualCameraPriority(_inspectCameras);
                    AssignCameraPriority(0, _inspectCameras);

                    _mainUI.ShowDialogWindow(_lookTarget.name, _lookTarget.transform, _lookTarget._cameraTransform.position, _lookTarget.ObjectDialog, yesAct: () =>
                    {
                        _centralSystem.SetGameState(GameState.inspect);
                        ResetAllVirtualCameraPriority(_inspectCameras);
                        AssignCameraPriority(1, _inspectCameras);
                    }, noAct: () => ExitVisitRoom());

                    //_mainUI.SetupPopupUI("Visit Platform", "Hello... Do you want to play inside my Room?", yesButtonEnabled: true, noButtonEnabled: true).SetupUIEvents(yesAction: () =>
                    // {
                    //     _centralSystem.SetGameState(GameState.inspect);
                    //     ResetAllVirtualCameraPriority(_inspectCameras);
                    //     AssignCameraPriority(1, _inspectCameras);
                    // }, noAction: () => ExitVisitRoom());
                    //StartCoroutine(_mainUI.ShowPopupIE(null));
                });
            });
        }

        void ExitVisitRoom()
        {
            _mainUI.FadeScreen(true, 1f, () => _mainUI.ToggleBlockScreen(true), () =>
            {
                _rootScene.SetActive(true);
                _indoorScene.SetActive(false);
                _centralSystem.SetGameState(GameState.gameplay);
                _mainUI.FadeScreen(false, .5f);
                _mainUI.ToggleBlockScreen(false);
                _thirdPersonCharacter.transform.position = _inspectStartPos.position;
                _thirdPersonCharacter.transform.rotation = _inspectStartPos.rotation;
                SetCameraToLastControlled();
            });
        }

        void SetCameraToLastControlled()
        {
            ResetAllVirtualCameraPriority(_worldCameras);
            AssignCameraPriority(_worldCameras.First((x) => x.CameraId == ControlledPlayer.CharacterId).CameraId, _worldCameras);
        }
    }
}