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
    [SerializeField] bool isAssignedOnStart;


    // Start is called before the first frame update
    void Start()
    {
        _centralSystem = FindObjectOfType<GameCentralSystem>();
        if (isAssignedOnStart)
        {
            AssignColors();
        }
    }

    /// <summary>
    /// Assigned on start or by events
    /// </summary>
    public void AssignColors()
    {
        var materialPack = _renderer.materials;
        for (int j = 0; j < _hairMaterialsIndex.Length; j++)
        {
            if (!isAssignedOnStart)
            {
                _centralSystem.SelectedHairMaterial.color = _renderer.materials[_hairMaterialsIndex[j]].color;
            }
            materialPack[_hairMaterialsIndex[j]] = _centralSystem.SelectedHairMaterial;
        }

        for (int j = 0; j < _eyeMaterialsIndex.Length; j++)
        {
            if (!isAssignedOnStart)
            {
                _centralSystem.SelectedEyeMaterial.color = _renderer.materials[_eyeMaterialsIndex[j]].color;
            }
            materialPack[_eyeMaterialsIndex[j]] = _centralSystem.SelectedEyeMaterial;
        }

        for (int j = 0; j < _clothesMaterialsIndex.Length; j++)
        {
            if (!isAssignedOnStart)
            {
                _centralSystem.SelectedClothesMaterial.color = _renderer.materials[_clothesMaterialsIndex[j]].color;
            }
            materialPack[_clothesMaterialsIndex[j]] = _centralSystem.SelectedClothesMaterial;
        }
        _renderer.materials = materialPack;
    }
}
