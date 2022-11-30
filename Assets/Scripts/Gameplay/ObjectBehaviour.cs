using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectBehaviour : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] GameHandler _gameHandler;
    [SerializeField] EventTrigger _eventTrigger;

    [field: SerializeField] public bool IsInspectable;
    public event Action<ObjectBehaviour> onHoverObject;
    public event Action<ObjectBehaviour> onExitHoverObject;
    public event Action<ObjectBehaviour> onInteractObject;

    private void OnDestroy()
    {
        onHoverObject = null;
        onExitHoverObject = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameHandler = FindObjectOfType<GameHandler>();

        _eventTrigger.AddEvent(EventTriggerType.PointerEnter, (data) =>
        {
            Debug.Log("Hovered on building : " + gameObject.name);
            onHoverObject?.Invoke(this);
        });

        _eventTrigger.AddEvent(EventTriggerType.PointerExit, (data) =>
        {
            Debug.Log("Exited on building : " + gameObject.name);
            onExitHoverObject?.Invoke(this);
        });

        _eventTrigger.AddEvent(EventTriggerType.PointerClick, (data) =>
        {
            if (_gameHandler.IsInspecting)
            {
                return;
            }
            onInteractObject?.Invoke(this);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
