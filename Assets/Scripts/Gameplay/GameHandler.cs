using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] UIControl _uiControl;
    [SerializeField] GameObject _playgroundParent;
    [SerializeField] InspectGroundBehaviour _inspectParent;

    [Header("Getter Setter Fields")]
    [field: SerializeField] public CharacterBehaviour[] Players;
    [field: SerializeField] public CameraBehaviour[] Cameras;
    [field: SerializeField] public ObjectBehaviour[] Buildings;
    [field: SerializeField] public ObjectBehaviour[] Objects;
    [field: SerializeField] public CharacterBehaviour ControlledPlayer { get
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
    [field: SerializeField] public bool IsInspecting { get; private set; }

    CharacterBehaviour _inspectedCharacter;
    ObjectBehaviour _inspectedObject;
    Quaternion _selectedRotationData;
    int _lastCameraPriority;
    bool _hasInspectCharacter;

    private void Start()
    {
        Debug.Log("Starting handler");
        InitObjects();
        InitEvents();
    }

    private void InitObjects()
    {
        foreach (var item in Buildings)
        {
            Debug.Log("Initializing Objects");
            item.onHoverObject += (info) => _uiControl.ToggleHoverInfo(info.gameObject).ToggleMouse(info, _uiControl.MousePivot);
            item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo();
            item.onInteractObject += (info) => info.OnObjectInspected(out _inspectedObject, out _selectedRotationData, _inspectParent.gameObject, _playgroundParent, _uiControl, () =>
            {
                SetCameraPriority(3, false);
                _hasInspectCharacter = false;
                _inspectParent.ToggleInspectionBackground(false);
                IsInspecting = true;
            });
        }

        foreach (var item in Objects)
        {
            item.onHoverObject += (info) => _uiControl.ToggleHoverInfo(info.gameObject).ToggleMouse(info, _uiControl.MousePivot);
            item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo();
        }
    }

    private void InitEvents()
    {
        _uiControl.onReturnInspectPressed += () =>
        {
            if (_hasInspectCharacter)
            {
                _inspectedCharacter.ExitInspectCharacter(_selectedRotationData, _inspectParent.gameObject, _playgroundParent, _uiControl, () =>
                {
                    SetCameraPriority(_lastCameraPriority, false);
                });
            }
            else
            {
                _inspectedObject.ExitInspectObject(_selectedRotationData, _inspectParent.gameObject, _playgroundParent, _uiControl, () =>
                {
                    SetCameraPriority(_lastCameraPriority, false);
                });
            }
            IsInspecting = false;
        };

        foreach (var item in Players)
        {
            Debug.Log("Creating event");
            item.InitCharacterEvents(this);
            item.onHoverObject += (info) => _uiControl.ToggleHoverInfo(info.gameObject).ToggleMouse(info, _uiControl.MousePivot);
            item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo();
            item.onSelectCharacter += OnChangedCharacted;
            item.onInteractCharacter += (info) => info.OnCharacterInspected(out _inspectedCharacter, out _selectedRotationData, _inspectParent.gameObject, _playgroundParent, _uiControl, () =>
            {
                SetCameraPriority(3, false);
                _hasInspectCharacter = true;
                _inspectParent.ToggleInspectionBackground(true);
                IsInspecting = true;
            });
        }
    }    

    public void OnChangedCharacted()
    {
        Debug.Log("Changed character");
        foreach (var item in Players)
        {
            item.ToggleSelected(false);
        }
        ResetAllVirtualCameraPriority();
    }

    public void SetCameraPriority(int comparedId, bool saveLastId = true)
    {
        ResetAllVirtualCameraPriority();
        foreach (var item in Cameras)
        {
            if (item.CameraId == comparedId)
            {
                item.SetCameraPriority(1);
                if(saveLastId)
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
}
