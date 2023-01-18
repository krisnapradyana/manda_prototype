using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singletons;

public class CharCustomizeablePartsApplier : MonoBehaviour
{
    GameCentralSystem _centralSystem;

    [SerializeField] GameObject _headBone;
    [SerializeField] GameObject _bodyBone;

    [SerializeField] bool _assignedOnStart;
    [SerializeField] bool _allowUpdate;

    // Start is called before the first frame update
    void Start()
    {
        _centralSystem = FindObjectOfType<GameCentralSystem>();

        if (_assignedOnStart)
        {
            ResizeParts();
            return;
        }

        _centralSystem.SelectedCharacterHeadSize = 1f;
        _centralSystem.SelectedCharacterBodySize = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_allowUpdate)
        {
            ResizeParts();
        }
    }

    void ResizeParts()
    {
        _headBone.transform.localScale = new Vector3(_centralSystem.SelectedCharacterHeadSize, _centralSystem.SelectedCharacterHeadSize, _centralSystem.SelectedCharacterHeadSize);
        _bodyBone.transform.localScale = new Vector3(_centralSystem.SelectedCharacterBodySize, _centralSystem.SelectedCharacterBodySize, _centralSystem.SelectedCharacterBodySize);
    }
}
