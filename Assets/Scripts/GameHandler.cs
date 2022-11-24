using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameHandler : MonoBehaviour
{
    public PlayerBehaviour[] _players;
    public CameraBehaviour[] _freelookCamera;

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
            item.onSelectCharacter += OnChangedCharacted;
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
