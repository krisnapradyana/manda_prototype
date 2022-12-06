using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IInteractableObject 
{
    public EventTrigger Trigger { get => Trigger; set => Trigger = value; }
    public bool IsInspectable { get; set; }
    public event Action<GameObject> onHoverObject;
    public event Action<GameObject> onExitHoverObject;
    public event Action<GameObject> onInteractObject;
}
