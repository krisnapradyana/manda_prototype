using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singletons;

enum ScaleType
{ 
    head,
    body
}

public class CharCustomizeableParts : MonoBehaviour
{
    [SerializeField] ScaleType scaleType;
    GameCentralSystem _centralSystem;
    private void Start()
    {
        _centralSystem = FindObjectOfType<GameCentralSystem>();
    }

    public void OnValueChanged(float value)
    {
        if (scaleType == ScaleType.head)
        {
            _centralSystem.SelectedCharacterHeadSize = value;
            Debug.Log("Head Size to : " + _centralSystem.SelectedCharacterHeadSize);
        }

        if (scaleType == ScaleType.body)
        {
            _centralSystem.SelectedCharacterBodySize = value;
            Debug.Log("Body Size to : " + _centralSystem.SelectedCharacterBodySize);
        }
    }
}
