using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTrigger : MonoBehaviour
{
    [field: SerializeField] public string targetName { get; private set; }
    [field: SerializeField] public Action onTriggered { get; private set; }

    public void InitTrigger(string tagName, Action trigger)
    {
        targetName = tagName;
        onTriggered = trigger;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == targetName)
        {
            Debug.Log("Object Triggered");
            onTriggered?.Invoke();
        }
    }
}
