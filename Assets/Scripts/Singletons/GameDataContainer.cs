using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataContainer : MonoBehaviour
{
    public static GameDataContainer GameData{ get; private set; }

    [field : SerializeField]
    public int SelectedCharacterIndex { get; set; }


    private void Awake()
    {
        if (GameData!= null)
        {
            return;
        }
        GameData = this;
        DontDestroyOnLoad(this);
    }
}
