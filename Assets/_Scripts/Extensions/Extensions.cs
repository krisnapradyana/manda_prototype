using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Gameplay;


public static class EventTriggerShortcut
{
    public static void AddEvent(this EventTrigger trigger, EventTriggerType triggerType, Action<PointerEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback.AddListener((data) => { action?.Invoke((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
}

public static class UIMouseVisibility
{
    public static GameplayUIControl ToggleMouse(this GameplayUIControl uiControl, CharacterBehaviour characterBehaviour, RectTransform mouseImage)
    {
        if (characterBehaviour != null)
        {
            if (characterBehaviour.IsNPC || !characterBehaviour.IsSelected)
            {
                mouseImage.gameObject.SetActive(true);
            }
        }
        return uiControl;
    }

    public static GameplayUIControl ToggleMouse(this GameplayUIControl uiControl, ObjectBehaviour objectBehaviour, RectTransform mouseImage)
    {
        if (objectBehaviour != null)
        {
            if (objectBehaviour.IsInspectable)
            {
                mouseImage.gameObject.SetActive(true);
            }
        }
        return uiControl;
    }
}
