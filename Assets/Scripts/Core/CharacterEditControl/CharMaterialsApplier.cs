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

    private void Awake()
    {
        _centralSystem = FindObjectOfType<GameCentralSystem>();
    }

    public void OnColorChanged(Color color)
    {
        switch (colorType)
        {
            case ColorType.hair:
                _centralSystem.SelectedHairMaterial.color = color;
                break;
            case ColorType.eye:
                _centralSystem.SelectedEyeMaterial.color = color;
                break;
            case ColorType.clothes:
                _centralSystem.SelectedClothesMaterial.color = color;
                break;
            default:
                break;
        }
    }
}