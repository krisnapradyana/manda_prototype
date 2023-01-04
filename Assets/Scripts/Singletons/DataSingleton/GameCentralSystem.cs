using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Singletons
{
    public class GameCentralSystem : MonoBehaviour
    {
        private GameState _currentState;
        public static GameCentralSystem GameData { get; private set; }
        public int SelectedCharacterIndex { get; private set; }
        public string PlayerName { get; private set; }
        public GameState CurrentState { get { return _currentState; } private set 
            {
                LastState = _currentState;
                _currentState = value;
            } 
        }
        public GameState LastState { get; private set; }

        private void Awake()
        {
            if (GameData != null)
            {
                return;
            }
            GameData = this;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
  
        }

        public void SelectCharacter(int targetIndex)
        {
            SelectedCharacterIndex = targetIndex;
        }

        public void SetPlayerName(string playerName)
        {
            PlayerName = playerName;
        }

        public void SetGameState(GameState currentState)
        {
            CurrentState = currentState;
        }

        public void SetSelectedPlatformId(int platformId)
        {

        }
    }
}