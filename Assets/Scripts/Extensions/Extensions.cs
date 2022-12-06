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

public static class AccessComponents
{
    /// <summary>
    /// Shorthand of GetComponent
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameobject"></param>
    /// <returns></returns>
    public static T Acquire<T> (this GameObject gameobject)
    {
        var aquiredClass = gameobject.GetComponent<T>();
        return aquiredClass;
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

public static class ObjectInspections
{
    public static void OnCharacterInspected
        (this CharacterBehaviour selectedObject, out CharacterBehaviour inspectedCharacter, out Quaternion selectedRotationData,
            GameObject inspectParent, GameObject playgroundParent, GameplayUIControl uiControl, Action additionalEvent = null)
    {
        inspectedCharacter = selectedObject;
        selectedRotationData = inspectedCharacter.transform.rotation;
        inspectParent.transform.position = new Vector3(inspectedCharacter.transform.position.x, 0, inspectedCharacter.transform.position.z);
        //inspectedCharacter.transform.rotation = Quaternion.Euler(0, -130, 0);
        inspectedCharacter.transform.parent = inspectParent.transform;

        playgroundParent.SetActive(false);
        inspectParent.SetActive(true);
        uiControl.ToggleExitButtonVisibility(true);
        additionalEvent?.Invoke();
        Debug.Log("Inspecting Character : " + selectedObject.name);
    }

    public static void OnObjectInspected
    (this ObjectBehaviour selectedObject, out ObjectBehaviour inspectedObject, out Quaternion selectedRotationData,
        GameObject inspectParent, GameObject playgroundParent, GameplayUIControl uiControl, Action additionalEvent = null)
    {
        inspectedObject = selectedObject;
        selectedRotationData = inspectedObject.transform.rotation;
        inspectParent.transform.position = new Vector3(inspectedObject.transform.position.x, 0, inspectedObject.transform.position.z);
        //inspectedObject.transform.rotation = Quaternion.Euler(0, -130, 0);
        inspectedObject.transform.parent = inspectParent.transform;

        playgroundParent.SetActive(false);
        inspectParent.SetActive(true);
        uiControl.ToggleExitButtonVisibility(true);
        additionalEvent?.Invoke();
        Debug.Log("Inspecting Object : " + selectedObject.name);
    }

    public static void ExitInspectCharacter
        (this CharacterBehaviour selectedObject, Quaternion _selectedRotationData,
            GameObject inspectParent, GameObject playgroundParent, GameplayUIControl uiControl, Action additionalEvent = null)
    {
        selectedObject.transform.rotation = _selectedRotationData;
        selectedObject.transform.parent = playgroundParent.transform;

        playgroundParent.SetActive(true);
        inspectParent.SetActive(false);
        uiControl.ToggleExitButtonVisibility(false);
        additionalEvent?.Invoke();
    }

    public static void ExitInspectObject
    (this ObjectBehaviour selectedObject, Quaternion _selectedRotationData,
            GameObject inspectParent, GameObject playgroundParent, GameplayUIControl uiControl, Action additionalEvent = null)
    {
        selectedObject.transform.rotation = _selectedRotationData;
        selectedObject.transform.parent = playgroundParent.transform;

        playgroundParent.SetActive(true);
        inspectParent.SetActive(false);
        uiControl.ToggleExitButtonVisibility(false);
        additionalEvent?.Invoke();
    }

}
