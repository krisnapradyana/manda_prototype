using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Singletons;

public enum ColorType
{
    hair,eye,clothes
}

public class CharMaterialsApplier : MonoBehaviour
{
    private GameCentralSystem _centralSystem;
    public ColorType colorType;

    private void Start()
    {
        _centralSystem = FindObjectOfType<GameCentralSystem>();
    }

    public void OnColorChanged(Color color)
    {
        switch (colorType)
        {
            case ColorType.hair:
                _centralSystem.selectedHairColor = color;
                break;
            case ColorType.eye:
                _centralSystem.selectedEyeColor = color;
                break;
            case ColorType.clothes:
                _centralSystem.selectedClothesColor = color;
                break;
            default:
                break;
        }
    }
}