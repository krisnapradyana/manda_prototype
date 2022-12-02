using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface InteractableObject 
{
    [SerializeField] EventTrigger Trigger { get; set; }
    [field: SerializeField] public bool IsInspectable { get; set; }
    public event Action<GameObject> onHoverObject;
    public event Action<GameObject> onExitHoverObject;
    public event Action<GameObject> onInteractObject;
}
