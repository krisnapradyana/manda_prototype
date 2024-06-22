using Gameplay;
using Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class WorldLevelGameHandler : RootHandler
{
    //Singleton Preferences
    [HideInInspector] public VRUI _vrUI { get; private set; }
    [Header("Scene Root Object")]
    [SerializeField] GameObject _indoorScene;
    [SerializeField] GameObject _rootScene;

    [Header("References")]
    [SerializeField] GameplayUIControl _uiControl;

    [Header("Level Objects Properties")]
    public CharacterBehaviour[] _npcs;
    public ObjectBehaviour[] _objects;

    private void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitObjects()
    {
        foreach (var item in _npcs)
        {
            item.InitCharacterEvents(this);
            item.onHoverObject += (info) =>
            {
                if (IsInspecting)
                {
                    _uiControl.ToggleHoverInfo();
                    return;
                }
                _uiControl.ToggleHoverInfo(info.gameObject).ToggleMouse(info.GetComponent<CharacterBehaviour>(), _uiControl.MousePivot);

            };
            item.onExitHoverObject += (info) => _uiControl.ToggleHoverInfo();
            item.onInteractObject += (info) =>
            {
                //Event for VRMode
                if (item.IsVRCharacter)
                {
                    if (_vrUI._centralSystem.IsCharacterSpeak)
                    {
                        Debug.Log("character currently speaking");
                        return;
                    }
                    //_vrUI.SetCurrentSelectedObject(item.gameObject);
                    _vrUI.ShowDialogWindow(item.name, item.transform, item._cameraTransform.position, item.GetDialogData());

                    return;
                }
            };
        }
}
