using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameHandler : MonoBehaviour
{
    [field: SerializeField] public PlayerBehaviour[] _players;
    [field: SerializeField] public CameraBehaviour[] _cameras;
    [field: SerializeField] public PlayerBehaviour _controlledPlayer { get
        {
            PlayerBehaviour result = null;
            foreach (var item in _players)
            {
                if (item._isSelected)
                {
                    result = item;
                }
            }
            return result;
        }
    }
 
    private void Start()
    {
        Debug.Log("Starting handler");
        InitEvents();
    }

    private void Update()
    {
        
    }

    private void InitEvents()
    {
        foreach (var item in _players)
        {
            Debug.Log("Creating event");
            item.InitCharacterEvents(this);
            item.onSelectCharacter += OnChangedCharacted;
        }
    }    

    public void OnChangedCharacted()
    {
        Debug.Log("Changed character");
        foreach (var item in _players)
        {
            item.ToggleSelected(false);
        }

        foreach (var item in _cameras)
        {
            item.SetCameraPriority(0);
        }
    }
}
