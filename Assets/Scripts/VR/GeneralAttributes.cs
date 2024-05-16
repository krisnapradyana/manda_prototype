using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAttributes : MonoBehaviour
{
    [Header("Meta Object")]
    public GameObject centerEyeObject;

    [Header("User GameObject")]
    public GameObject TopAnchor;
    public GameObject panelParent, panelControl;
    public GameObject pivotLeftController, pivotLeftHand;

    [Header("Canvas")]
    public Canvas FloatingUI;
}
