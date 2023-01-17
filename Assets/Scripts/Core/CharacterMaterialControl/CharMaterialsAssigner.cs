using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singletons;

public class CharMaterialsAssigner : MonoBehaviour
{
    private GameCentralSystem _centralSystem;

    [SerializeField] Material[] _hairMaterials;
    [SerializeField] Material[] _eyeMaterials;
    [SerializeField] Material[] _clothesMaterials;
    [SerializeField] bool isRealtime;


    // Start is called before the first frame update
    void Start()
    {
        _centralSystem = FindObjectOfType<GameCentralSystem>();
        AssignColors();
    }

    private void Update()
    {
        AssignColors();   
    }

    void AssignColors()
    {
        foreach (var item in _hairMaterials)
        {
            item.color = _centralSystem.selectedHairColor;
        }
        foreach (var item in _eyeMaterials)
        {
            item.color = _centralSystem.selectedEyeColor;
        }
        foreach (var item in _clothesMaterials)
        {
            item.color = _centralSystem.selectedClothesColor;
        }
    }
}
