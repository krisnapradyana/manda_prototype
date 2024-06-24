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
    [SerializeField] float rayDistance;

    RaycastHit hit;

    private void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitObjects();
    }

    // Update is called once per frame
    void Update()
    {
        DrawRayFromCamera();
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

    void DrawRayFromCamera()
    {
        // Create a ray from the camera's position and forward direction
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
      
        // Perform the raycast
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Log the name of the object hit
            Debug.Log("Hit object: " + hit.collider.gameObject.name);

            // Optional: draw a line in the editor to visualize the raycast
            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.001f);
            GetCollidedObject(hit);
        }
        else
        {
            // Draw the ray in the Scene view for debugging purposes
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);
        }
    }

    void GetCollidedObject(RaycastHit hit)
    {
        if (hit.collider.gameObject.GetComponent<Interactables>())
        {
            Debug.Log("YEZZ");
        }
        else
        {
            Debug.Log("NOOO");
        }
    }

    void ShowUIOnObjects()
    {

    }
}
