using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectGroundBehaviour : MonoBehaviour
{
    [SerializeField] GameObject[] _inspectAreaObjects;
    public void ToggleInspectionBackground(bool visibility)
    {
        foreach (var item in _inspectAreaObjects)
        {
            item.SetActive(visibility);
        }
    }
}
