using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataContainer : MonoBehaviour
{
    public static GameDataContainer GameData{ get; private set; }
    public int SelectedCharacterIndex { get; private set; }
    public string PlayerName { get; private set; }

    private void Awake()
    {
        if (GameData!= null)
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
}
