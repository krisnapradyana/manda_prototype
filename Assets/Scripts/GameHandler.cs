using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameHandler : MonoBehaviour
{
    [field: SerializeField] public PlayerBehaviour[] _players;
    [field: SerializeField] public CameraBehaviour[] _freelookCamera;
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
        InitEvents();
    }

    private void Update()
    {
        
    }

    private void InitEvents()
    {
        foreach (var item in _players)
        {
            item.InitCharacterEvents(this);
            //item.onMoveClicked
            //item.onSelectCharacter += OnChangedCharacted;
        }
    }    

    public void OnChangedCharacted()
    {
        foreach (var item in _players)
        {
            item.ToggleSelected(false);
        }

        foreach (var item in _freelookCamera)
        {
            item.SetCameraPriority(0);
        }
    }
}
