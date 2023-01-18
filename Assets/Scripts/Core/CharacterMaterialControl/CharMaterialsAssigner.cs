using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singletons;

public class CharMaterialsAssigner : MonoBehaviour
{
    private GameCentralSystem _centralSystem;

    [SerializeField] SkinnedMeshRenderer _renderer;
    [SerializeField] int[] _hairMaterialsIndex;
    [SerializeField] int[] _eyeMaterialsIndex;
    [SerializeField] int[] _clothesMaterialsIndex;
    [SerializeField] bool isRealtime;


    // Start is called before the first frame update
    void Start()
    {
        _centralSystem = FindObjectOfType<GameCentralSystem>();
        AssignColors();
    }

    void AssignColors()
    {
        var materialPack = _renderer.materials;
        for (int j = 0; j < _hairMaterialsIndex.Length; j++)
        {
            materialPack[_hairMaterialsIndex[j]] = _centralSystem.selectedHairMaterial;
        }

        for (int j = 0; j < _eyeMaterialsIndex.Length; j++)
        {
            materialPack[_eyeMaterialsIndex[j]] = _centralSystem.selectedEyeMaterial;
        }

        for (int j = 0; j < _clothesMaterialsIndex.Length; j++)
        {
            materialPack[_clothesMaterialsIndex[j]] = _centralSystem.selectedClothesMaterial;
        }


        _renderer.materials = materialPack;
    }
}
