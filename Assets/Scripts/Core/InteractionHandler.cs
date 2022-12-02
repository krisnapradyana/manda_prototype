using Gameplay;
using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InteractionHandler
{
    public enum InspectObjectType
    {
        characters,
        objects,
        selectionObjects,
    }

    public static T OnObjectInspected<T> (this T reference, InspectObjectType type,out Type targetObject, out Quaternion selectedRotationData, GameObject inspectParent = null, GameObject playgroundParent = null, UIControl uiControl = null, Action additionalEvent = null)
    {
        dynamic inspectedObject = null;
        targetObject = null;
        switch (type)
        {
            case InspectObjectType.characters:
                targetObject = inspectedObject = reference as CharacterBehaviour;
                break;
            case InspectObjectType.objects:
                targetObject = inspectedObject = reference as ObjectBehaviour;
                break;
            case InspectObjectType.selectionObjects:
                targetObject = inspectedObject = reference as SelectableCharacter;
                break;
            default:
                break;
        }

        selectedRotationData = inspectedObject.transform.rotation;
        inspectParent.transform.position = new Vector3(inspectedObject.transform.position.x, 0, inspectedObject.transform.position.z);
        inspectedObject.transform.rotation = Quaternion.Euler(0, -130, 0);
        inspectedObject.transform.parent = inspectParent.transform;

        playgroundParent.SetActive(false);
        inspectParent.SetActive(true);
        uiControl.ToggleExitButtonVisibility(true);
        additionalEvent?.Invoke();
        Debug.Log("Inspecting Object : " + inspectedObject.name);
        return reference;
    }

}
