using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IInteractableObject 
{
    public ObjectType Type { get; }
    public bool IsInspectable { get; set; }
    public Action<GameObject> onHoverObject { get; set; }
    public Action<GameObject> onExitHoverObject { get; set; }
    public Action<GameObject> onInteractObject { get; set; }
}
