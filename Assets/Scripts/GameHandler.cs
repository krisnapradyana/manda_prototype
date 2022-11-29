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

    [Header("Getter Setter Fields")]
    [field: SerializeField] public PlayerBehaviour[] Players;
    [field: SerializeField] public CameraBehaviour[] Cameras;
    [field: SerializeField] public BuildingBehaviour[] Buildings;
    [field: SerializeField] public PlayerBehaviour ControlledPlayer { get
        {
            PlayerBehaviour result = null;
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
        foreach (var item in Players)
        {
            Debug.Log("Creating event");
            item.InitCharacterEvents(this);
            item.onHoverObject += (info) => _uiControl.ToggleHoverInfo(true, info.gameObject);
            item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo(false);
            item.onSelectCharacter += OnChangedCharacted;
        }
    }    

    public void OnChangedCharacted()
    {
        Debug.Log("Changed character");
        foreach (var item in Players)
        {
            item.ToggleSelected(false);
        }

        foreach (var item in Cameras)
        {
            item.SetCameraPriority(0);
        }
    }
}
