using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralAttributes : MonoBehaviour
{
    [Header("Meta Object")]
    public GameObject xrOrigin;
    public GameObject centerEyeObject;

    [Header("Additional GameObject")]
    public GameObject playerChar;
    public Transform xrPrevPos, startingPosition, initialPoint;
    public GameObject pivotLeftController, pivotLeftHand, pivotRightController, pivotRightHand;
    public Image darkOverlay, lightOverlay;
}
