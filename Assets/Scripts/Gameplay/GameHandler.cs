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
    [SerializeField] GameObject _inspectParent;

    [Header("Getter Setter Fields")]
    [field: SerializeField] public CharacterBehaviour[] Players;
    [field: SerializeField] public CameraBehaviour[] Cameras;
    [field: SerializeField] public BuildingBehaviour[] Buildings;
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

    CharacterBehaviour _inspectedCharacter;
    Quaternion _selectedRotationData;
    int _lastCameraPriority;

    private void OnDestroy()
    {
   
    }

    private void Start()
    {
        Debug.Log("Starting handler");
        InitObjects();
        InitEvents();
    }

    private void Update()
    {
        
    }

    private void InitObjects()
    {
        foreach (var item in Buildings)
        {
            Debug.Log("Initializing Objects");
            item.onHoverObject += (info) => _uiControl.ToggleHoverInfo(true, info.gameObject);
            item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo(false);
        }
    }

    private void InitEvents()
    {
        _uiControl.onReturnInspectPressed += ()=> ExitInpectCharacter(_inspectedCharacter);

        foreach (var item in Players)
        {
            Debug.Log("Creating event");
            item.InitCharacterEvents(this);
            item.onHoverObject += (info) => _uiControl.ToggleHoverInfo(true, info.gameObject);
            item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo(false);
            item.onSelectCharacter += OnChangedCharacted;
            item.onInteractCharacter += (character) => OnCharacterInspected(character);
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

    public void OnCharacterInspected(CharacterBehaviour character)
    {
        _inspectedCharacter = character;
        _selectedRotationData = _inspectedCharacter.transform.rotation;
        _inspectParent.transform.position = new Vector3(_inspectedCharacter.transform.position.x, 0, _inspectedCharacter.transform.position.z);
        _inspectedCharacter.transform.rotation = Quaternion.Euler(0, -130, 0);
        _inspectedCharacter.transform.parent = _inspectParent.transform;

        _playgroundParent.SetActive(false);
        _inspectParent.SetActive(true);
        SetCameraPriority(3,false);
        Debug.Log("Inspecting Character : " + character.name);
    }

    public void ExitInpectCharacter(CharacterBehaviour character)
    {
        character.transform.rotation = _selectedRotationData;
        character.transform.parent = _playgroundParent.transform;

        _playgroundParent.SetActive(true);
        _inspectParent.SetActive(false);

        SetCameraPriority(_lastCameraPriority, false);
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
