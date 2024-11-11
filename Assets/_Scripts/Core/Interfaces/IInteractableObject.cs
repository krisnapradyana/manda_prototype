using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gameplay;

public interface IInteractableObject 
{
    public ObjectType Type { get; }
    public bool IsInspectable { get; set; }
    public Action<Interactables> onHoverObject { get; set; }
    public Action<Interactables> onExitHoverObject { get; set; }
    public Action<Interactables> onInteractObject { get; set; }
}
